

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.UpdateReview
{
    public class UpdateReviewHandler : IRequestHandler<UpdateReviewCommand, Result<ReviewDto>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateReviewHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<ReviewDto>> Handle(UpdateReviewCommand request, CancellationToken ct)
        {
            var review = await _uow.Reviews.GetByIdAsync(request.ReviewId, ct);
            if (review == null) return Result<ReviewDto>.NotFound("Review not found.");
            if (review.StudentId != request.StudentId) return Result<ReviewDto>.Forbidden();

            if (request.Dto.Rating != null) review.Rating = request.Dto.Rating.Value;
            if (request.Dto.Comment != null) review.Comment = request.Dto.Comment;
            review.Status = ReviewStatus.Pending; // re-moderate on update

            await _uow.Reviews.UpdateAsync(review);
            await _uow.SaveChangesAsync(ct);

            var student = await _uow.Users.GetByIdAsync(review.StudentId, ct);
            var course = await _uow.Courses.GetByIdAsync(review.CourseId, ct);

            return Result<ReviewDto>.Success(new ReviewDto(
                review.Id, review.CourseId, course?.Title ?? string.Empty,
                review.StudentId, student?.FullName ?? string.Empty, student?.AvatarUrl,
                review.Rating, review.Comment, review.Status.ToString(),
                review.HelpfulCount, new(), review.CreatedAt));
        }
    }
}
