
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.Login
{
    public record LoginCommand(LoginDto Dto) : IRequest<Result<AuthResponseDto>>;

}
