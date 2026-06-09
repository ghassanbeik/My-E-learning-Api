

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Auth.VerifyEmail
{
    public record VerifyEmailCommand(Guid UserId, string Token) : IRequest<Result>;

}
