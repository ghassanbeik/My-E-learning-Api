

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Auth.UpdateProfile
{
    public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, Result<UserProfileDto>>
    {
        private readonly IUnitOfWork _uow;

        public UpdateProfileHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<UserProfileDto>> Handle(UpdateProfileCommand request, CancellationToken ct)
        {
            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<UserProfileDto>.NotFound("User not found.");

            if (request.Dto.FirstName != null) user.FirstName = request.Dto.FirstName;
            if (request.Dto.LastName != null) user.LastName = request.Dto.LastName;
            if (request.Dto.Bio != null) user.Bio = request.Dto.Bio;
            if (request.Dto.Headline != null) user.Headline = request.Dto.Headline;
            if (request.Dto.Website != null) user.Website = request.Dto.Website;
            if (request.Dto.Twitter != null) user.Twitter = request.Dto.Twitter;
            if (request.Dto.LinkedIn != null) user.LinkedIn = request.Dto.LinkedIn;
            if (request.Dto.YouTube != null) user.YouTube = request.Dto.YouTube;

            await _uow.Users.UpdateAsync(user);
            await _uow.SaveChangesAsync(ct);

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(user.Id, ct)).ToList();
            var enrollments = await _uow.Enrollments.GetByStudentAsync(user.Id, ct);

            return Result<UserProfileDto>.Success(new UserProfileDto(
                user.Id, user.FirstName, user.LastName, user.FullName, user.Email,
                user.AvatarUrl, user.Bio, user.Headline, user.Website,
                user.Twitter, user.LinkedIn, user.YouTube,
                user.IsEmailVerified, user.LastLoginAt, roles,
                enrollments.Count(),
                enrollments.Count(e => e.Status == Domain.Enums.EnrollmentStatus.Completed),
                (await _uow.Certificates.GetByStudentAsync(user.Id, ct)).Count()));
        }
    }

}
