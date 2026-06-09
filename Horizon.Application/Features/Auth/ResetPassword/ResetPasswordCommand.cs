
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.ResetPassword
{
    public record ResetPasswordCommand(ResetPasswordDto Dto) : IRequest<Result>;

}
