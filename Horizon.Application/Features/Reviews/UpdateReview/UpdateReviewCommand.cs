

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Reviews.UpdateReview
{
    public record UpdateReviewCommand(Guid ReviewId, Guid StudentId, UpdateReviewDto Dto) : IRequest<Result<ReviewDto>>;

}
