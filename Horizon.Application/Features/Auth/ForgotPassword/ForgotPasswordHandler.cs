
using Horizon.Application.Common;
using Horizon.Domain.Events.AuthEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Auth.ForgotPassword
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public ForgotPasswordHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByEmailAsync(request.Email, ct);
            if (user == null) return Result.Success(); // Don't reveal if email exists

            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var resetLink = $"https://horizon.com/reset-password?token={token}&email={user.Email}";

            await _eventBus.PublishAsync(new PasswordResetRequestedEvent
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                ResetLink = resetLink,
            }, ct);

            return Result.Success();
        }
    }
}
