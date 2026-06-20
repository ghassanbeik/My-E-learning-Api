using Horizon.Application.Common;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;
using System.Security.Cryptography;
using System.Text;

namespace Horizon.Application.Features.Auth.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);

        public ForgotPasswordHandler(IUnitOfWork uow, IEventBus eventBus)
        {
            _uow      = uow;
            _eventBus = eventBus;
        }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Email, ct);

            // Return Success regardless of whether the email exists — prevents
            // user-enumeration attacks (cannot distinguish "no account" vs "email sent").
            if (user == null) return Result.Success();

            // Invalidate any previous, still-valid tokens so only the
            // most recently issued link works.
            await _uow.VerificationTokens.InvalidateActiveTokensAsync(
                user.Id, VerificationTokenType.PasswordReset, ct);

            // 32 bytes = 256-bit token — cryptographically random.
            // Only the SHA-256 hash is stored in the DB.
            // Even a full DB dump cannot recover the raw token.
            var rawToken  = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            var tokenHash = HashToken(rawToken);

            await _uow.VerificationTokens.AddAsync(new VerificationToken
            {
                UserId    = user.Id,
                Type      = VerificationTokenType.PasswordReset,
                TokenHash = tokenHash,
                ExpiresAt = DateTime.UtcNow.Add(TokenLifetime),
            }, ct);

            await _uow.SaveChangesAsync(ct);

            var resetLink =
                $"https://horizon.com/reset-password" +
                $"?token={Uri.EscapeDataString(rawToken)}" +
                $"&email={Uri.EscapeDataString(user.Email)}";

            await _eventBus.PublishAsync(new PasswordResetRequestedEvent
            {
                UserId    = user.Id,
                Email     = user.Email,
                FullName  = user.FullName,
                ResetLink = resetLink,
            }, ct);

            return Result.Success();
        }

        // SHA-256 hex string — shared with ResetPasswordHandler via internal visibility.
        internal static string HashToken(string rawToken)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawToken));
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
