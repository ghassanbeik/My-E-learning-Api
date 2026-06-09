
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.GetCurrentUser
{
    public record GetCurrentUserQuery(Guid UserId) : IRequest<Result<UserProfileDto>>;

}
