

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Reviews.VoteReview
{
    public record VoteReviewCommand(Guid ReviewId, Guid UserId, bool IsHelpful) : IRequest<Result>;

}
