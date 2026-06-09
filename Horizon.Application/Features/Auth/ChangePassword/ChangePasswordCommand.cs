
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.ChangePassword
{
    public record ChangePasswordCommand(Guid UserId, ChangePasswordDto Dto) : IRequest<Result>;

}
