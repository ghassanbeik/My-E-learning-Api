

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Auth.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetUserByIdHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken ct)
        {
            var user = await _uow.Users.GetWithRolesAsync(request.UserId, ct);
            if (user == null)
                return Result<UserDto>.NotFound("User not found.");

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(user.Id, ct)).ToList();

            return Result<UserDto>.Success(new UserDto(
                user.Id,
                user.FirstName,
                user.LastName,
                user.FullName,
                user.Email,
                user.AvatarUrl,
                user.Headline,
                user.IsEmailVerified,
                roles));
        }
    }
}
