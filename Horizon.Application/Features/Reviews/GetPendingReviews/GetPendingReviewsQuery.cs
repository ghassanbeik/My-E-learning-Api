

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Reviews.GetPendingReviews
{
    public record GetPendingReviewsQuery(int Page = 1, int PageSize = 20) : IRequest<Result<PagedResponse<ReviewDto>>>;

}
