
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.CreateReviewResponse
{
    public class CreateReviewResponseHandler
       : IRequestHandler<CreateReviewResponseCommand, Result<ReviewResponseDto>>
    {
        private readonly IUnitOfWork _uow;

        public CreateReviewResponseHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<ReviewResponseDto>> Handle(
            CreateReviewResponseCommand request, CancellationToken ct)
        {
            var review = await _uow.Reviews.GetByIdAsync(request.ReviewId, ct);
            if (review == null) return Result<ReviewResponseDto>.NotFound("Review not found.");

            var course = await _uow.Courses.GetByIdAsync(review.CourseId, ct);
            if (course?.InstructorId != request.InstructorId)
                return Result<ReviewResponseDto>.Forbidden(
                    "Only the course instructor can respond to reviews.");

            var instructor = await _uow.Users.GetByIdAsync(request.InstructorId, ct);

            var response = new ReviewResponse
            {
                ReviewId = request.ReviewId,
                ResponderId = request.InstructorId,
                Response = request.Dto.Response,
            };

            await _uow.ReviewResponses.AddAsync(response, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<ReviewResponseDto>.Success(new ReviewResponseDto(
                response.Id,
                response.ResponderId,
                instructor?.FullName ?? string.Empty,
                instructor?.AvatarUrl,
                response.Response,
                response.CreatedAt), 201);
        }
    }
}
