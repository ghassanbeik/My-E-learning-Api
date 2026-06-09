

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Reviews.GetRatingDistribution
{
    public record GetRatingDistributionQuery(Guid CourseId) : IRequest<Result<Dictionary<int, int>>>;

}
