

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Auth.Logout
{
    public record LogoutCommand(string RefreshToken) : IRequest<Result>;

}
