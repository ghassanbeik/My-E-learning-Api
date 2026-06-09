
using Horizon.Application.Common;
using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.PasswordHasher;
using MediatR;

namespace Horizon.Application.Features.Auth.ChangePassword
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPasswordHasher _hasher;
        private readonly IEventBus _eventBus;

        public ChangePasswordHandler(IUnitOfWork uow, IPasswordHasher hasher, IEventBus eventBus)
        {
            _uow = uow;
            _hasher = hasher;
            _eventBus = eventBus;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result.NotFound("User not found.");

            if (!_hasher.Verify(request.Dto.CurrentPassword, user.PasswordHash))
                return Result.Failure("Current password is incorrect.");

            user.PasswordHash = _hasher.Hash(request.Dto.NewPassword);
            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new PasswordChangedEvent
            {
                UserId = user.Id,
                Email = user.Email,
            }, ct);

            return Result.Success();
        }
    }

}
