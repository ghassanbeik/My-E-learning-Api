

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Auth.VerifyEmail
{
    public class VerifyEmailHandler : IRequestHandler<VerifyEmailCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public VerifyEmailHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result.NotFound("User not found.");

            if (user.IsEmailVerified)
                return Result.Failure("Email is already verified.");

            if (string.IsNullOrWhiteSpace(request.Token))
                return Result.Failure("Invalid verification token.");

            user.IsEmailVerified = true;
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
