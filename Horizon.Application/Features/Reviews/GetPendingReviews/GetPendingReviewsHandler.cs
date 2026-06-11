

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.GetPendingReviews
{
    public class GetPendingReviewsHandler
         : IRequestHandler<GetPendingReviewsQuery, Result<PagedResponse<ReviewDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetPendingReviewsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<ReviewDto>>> Handle(
            GetPendingReviewsQuery request, CancellationToken ct)
        {
            var reviews = await _uow.Reviews.GetPendingAsync(ct);
            var items = reviews.Select(r => new ReviewDto(
                r.Id, r.CourseId, r.Course?.Title ?? string.Empty,
                r.StudentId, r.Student?.FullName ?? string.Empty, r.Student?.AvatarUrl,
                r.Rating, r.Comment, r.Status.ToString(), r.HelpfulCount,
                new(), r.CreatedAt)).ToList();

            var paged = items.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            return Result<PagedResponse<ReviewDto>>.Success(
                PagedResponse<ReviewDto>.From(paged, items.Count, request.Page, request.PageSize));
        }
    }
}
