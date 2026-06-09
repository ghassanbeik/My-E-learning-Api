

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Auth.UpdateProfile
{
    public record UpdateProfileCommand(Guid UserId, UpdateProfileDto Dto) : IRequest<Result<UserProfileDto>>;

}
