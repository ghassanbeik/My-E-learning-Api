

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Reviews.DeleteReview
{
    public record DeleteReviewCommand(Guid ReviewId, Guid StudentId) : IRequest<Result>;

}
