

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.ReviewEvents;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.CreateReview
{
    public class CreateReviewHandler : IRequestHandler<CreateReviewCommand, Result<ReviewDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public CreateReviewHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<ReviewDto>> Handle(CreateReviewCommand request, CancellationToken ct)
        {
            if (!await _uow.Enrollments.IsEnrolledAsync(request.StudentId, request.Dto.CourseId, ct))
                return Result<ReviewDto>.Forbidden("Must be enrolled to review this course.");

            if (await _uow.Reviews.HasReviewedAsync(request.StudentId, request.Dto.CourseId, ct))
                return Result<ReviewDto>.Conflict("Already reviewed this course.");

            var course = await _uow.Courses.GetByIdAsync(request.Dto.CourseId, ct);
            if (course == null) return Result<ReviewDto>.NotFound("Course not found.");

            var student = await _uow.Users.GetByIdAsync(request.StudentId, ct);
            if (student == null) return Result<ReviewDto>.NotFound("Student not found.");

            var review = new Review
            {
                CourseId = request.Dto.CourseId,
                StudentId = request.StudentId,
                Rating = request.Dto.Rating,
                Comment = request.Dto.Comment,
                Status = ReviewStatus.Pending,
            };

            await _uow.Reviews.AddAsync(review, ct);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new ReviewSubmittedEvent
            {
                ReviewId = review.Id,
                StudentId = request.StudentId,
                CourseId = course.Id,
                InstructorId = course.InstructorId,
                StudentName = student.FullName,
                CourseTitle = course.Title,
                Rating = review.Rating,
            }, ct);

            return Result<ReviewDto>.Success(new ReviewDto(
                review.Id, course.Id, course.Title, student.Id, student.FullName,
                student.AvatarUrl, review.Rating, review.Comment,
                review.Status.ToString(), 0, new(), review.CreatedAt), 201);
        }
    }
}
