

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Reviews.ApproveReview
{
    public record ApproveReviewCommand(Guid ReviewId) : IRequest<Result>;

}
