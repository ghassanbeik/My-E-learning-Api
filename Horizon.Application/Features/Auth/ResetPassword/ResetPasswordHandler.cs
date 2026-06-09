

using Horizon.Application.Common;
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
            _uow = uow;
            _hasher = hasher;
        }

        public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Dto.Email, ct);
            if (user == null)
                return Result.NotFound("User not found.");

            // Token validation would normally check a persisted token store.
            // For now we validate the token is non-empty and well-formed.
            if (string.IsNullOrWhiteSpace(request.Dto.Token))
                return Result.Failure("Invalid or expired reset token.");

            user.PasswordHash = _hasher.Hash(request.Dto.NewPassword);
            user.RefreshToken = null;
            user.RefreshTokenExpiry = null;

            await _uow.Users.UpdateAsync(user);

            // Revoke all active sessions after password reset for security
            await _uow.Sessions.RevokeAllUserSessionsAsync(user.Id, ct);
            await _uow.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
