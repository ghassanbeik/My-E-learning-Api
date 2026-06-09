

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Auth.GetCurrentUser
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, Result<UserProfileDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetCurrentUserHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<UserProfileDto>> Handle(GetCurrentUserQuery request, CancellationToken ct)
        {
            var user = await _uow.Users.GetFullProfileAsync(request.UserId, ct);
            if (user == null) return Result<UserProfileDto>.NotFound("User not found.");

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(user.Id, ct)).ToList();
            var enrollments = await _uow.Enrollments.GetByStudentAsync(user.Id, ct);
            var certs = await _uow.Certificates.GetByStudentAsync(user.Id, ct);

            return Result<UserProfileDto>.Success(new UserProfileDto(
                user.Id, user.FirstName, user.LastName, user.FullName, user.Email,
                user.AvatarUrl, user.Bio, user.Headline, user.Website,
                user.Twitter, user.LinkedIn, user.YouTube,
                user.IsEmailVerified, user.LastLoginAt, roles,
                enrollments.Count(),
                enrollments.Count(e => e.Status == Domain.Enums.EnrollmentStatus.Completed),
                certs.Count()));
        }
    }

}
