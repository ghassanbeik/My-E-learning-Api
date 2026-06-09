

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.DiscussionEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.CreateDiscussion
{
    public class CreateDiscussionHandler : IRequestHandler<CreateDiscussionCommand, Result<DiscussionDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public CreateDiscussionHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<DiscussionDto>> Handle(CreateDiscussionCommand request, CancellationToken ct)
        {
            if (!await _uow.Enrollments.IsEnrolledAsync(request.UserId, request.Dto.CourseId, ct))
            {
                var roles = await _uow.UserRoles.GetUserRoleNamesAsync(request.UserId, ct);
                if (!roles.Contains("Instructor") && !roles.Contains("Admin"))
                    return Result<DiscussionDto>.Forbidden("Must be enrolled to post in this course.");
            }

            var course = await _uow.Courses.GetByIdAsync(request.Dto.CourseId, ct);
            if (course == null) return Result<DiscussionDto>.NotFound("Course not found.");

            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<DiscussionDto>.NotFound("User not found.");

            var discussion = new Discussion
            {
                CourseId = request.Dto.CourseId,
                LessonId = request.Dto.LessonId,
                UserId = request.UserId,
                Type = Enum.Parse<DiscussionType>(request.Dto.Type),
                Title = request.Dto.Title,
                Content = request.Dto.Content,
            };

            await _uow.Discussions.AddAsync(discussion, ct);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new DiscussionCreatedEvent
            {
                DiscussionId = discussion.Id,
                CourseId = course.Id,
                InstructorId = course.InstructorId,
                StudentName = user.FullName,
                CourseTitle = course.Title,
                DiscussionTitle = discussion.Title,
            }, ct);

            return Result<DiscussionDto>.Success(new DiscussionDto(
                discussion.Id, course.Id, course.Title, discussion.LessonId, null,
                user.Id, user.FullName, user.AvatarUrl, discussion.Type.ToString(),
                discussion.Title, discussion.Content, false, false, 0, 0, new(), discussion.CreatedAt), 201);
        }
    }

}
