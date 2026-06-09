

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Reviews.GetCourseReviews
{
    public record GetCourseReviewsQuery(Guid CourseId, int Page = 1, int PageSize = 10) : IRequest<Result<PagedResponse<ReviewDto>>>;

}
