
using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Auth.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public LogoutHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(LogoutCommand request, CancellationToken ct)
        {
            await _uow.Sessions.RevokeSessionAsync(request.RefreshToken, string.Empty, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
