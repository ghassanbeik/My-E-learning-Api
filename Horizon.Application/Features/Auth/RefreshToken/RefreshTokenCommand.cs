

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.RefreshToken
{
    public record RefreshTokenCommand(RefreshTokenDto Dto) : IRequest<Result<AuthResponseDto>>;

}
