

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.Register
{
    public record RegisterCommand(RegisterDto Dto) : IRequest<Result<AuthResponseDto>>;

}
