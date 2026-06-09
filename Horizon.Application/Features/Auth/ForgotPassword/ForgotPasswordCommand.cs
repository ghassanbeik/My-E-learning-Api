

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Auth.ForgotPassword
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result>;

}
