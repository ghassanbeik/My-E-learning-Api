using Horizon.Application.Common;
using Horizon.Application.Features.Auth.ForgotPassword;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.PasswordHasher;
using MediatR;

namespace Horizon.Application.Features.Auth.ResetPassword
{
    public class ResetPasswordHandler : IRequestHandler<ResetPasswordCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;

        public ResetPasswordHandler(IUnitOfWork uow, IPasswordHasher hasher)
        {
            _uow    = uow;
            _hasher = hasher;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Dto.Email, ct);
            if (user == null)
                return Result.NotFound("User not found.");

            // Hash the raw token the client supplied, then look it up in the DB.
            // We never store raw tokens — only SHA-256 hashes — so even a full
            // DB dump cannot be used to construct a valid reset URL.
            var tokenHash = ForgotPasswordHandler.HashToken(request.Dto.Token);

            var verificationToken = await _uow.VerificationTokens.GetValidTokenAsync(
                tokenHash, VerificationTokenType.PasswordReset, ct);

            // "Invalid or expired" is intentionally vague — we don't reveal
            // whether the token never existed, was already used, or has expired.
            if (verificationToken == null)
                return Result.Failure("Invalid or expired password reset link.", 400);

            // Ensure the token belongs to the user in the request body.
            // Without this, an attacker with a valid token for their own account
            // could supply a victim's email address and take over that account.
            if (verificationToken.UserId != user.Id)
                return Result.Failure("Invalid or expired password reset link.", 400);

            // Consume the token — it cannot be replayed.
            verificationToken.UsedAt = DateTime.UtcNow;

            // Update the password and remove all session material.
            user.PasswordHash       = _hasher.Hash(request.Dto.NewPassword);
            user.RefreshToken       = null;
            user.RefreshTokenExpiry = null;

            await _uow.Users.UpdateAsync(user);

            // Kick out all active sessions for this user immediately.
            // If credentials were stolen, this invalidates the attacker's
            // session the moment the password is reset.
            await _uow.Sessions.RevokeAllUserSessionsAsync(user.Id, ct);

            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
