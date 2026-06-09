

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Reviews.RejectReview
{
    public record RejectReviewCommand(Guid ReviewId) : IRequest<Result>;

}
