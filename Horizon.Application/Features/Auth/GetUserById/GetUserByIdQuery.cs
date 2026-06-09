

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.GetUserById
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto>>;

}
