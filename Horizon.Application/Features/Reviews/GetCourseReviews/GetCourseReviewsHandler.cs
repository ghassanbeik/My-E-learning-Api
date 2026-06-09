
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.GetCourseReviews
{
    public class GetCourseReviewsHandler : IRequestHandler<GetCourseReviewsQuery, Result<PagedResponse<ReviewDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetCourseReviewsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<ReviewDto>>> Handle(GetCourseReviewsQuery request, CancellationToken ct)
        {
            var reviews = await _uow.Reviews.GetByCourseAsync(request.CourseId, ReviewStatus.Approved, ct);
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);

            var items = reviews.Select(r => new ReviewDto(
                r.Id, r.CourseId, course?.Title ?? string.Empty, r.StudentId,
                r.Student.FullName, r.Student.AvatarUrl, r.Rating, r.Comment,
                r.Status.ToString(), r.HelpfulCount, new(), r.CreatedAt)).ToList();

            var paged = items.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            return Result<PagedResponse<ReviewDto>>.Success(
                PagedResponse<ReviewDto>.From(paged, items.Count, request.Page, request.PageSize));
        }
    }
}
