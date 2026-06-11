

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.GetRatingDistribution
{
    public class GetRatingDistributionHandler
           : IRequestHandler<GetRatingDistributionQuery, Result<Dictionary<int, int>>>
    {
        private readonly IUnitOfWork _uow;
        public GetRatingDistributionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<Dictionary<int, int>>> Handle(
            GetRatingDistributionQuery request, CancellationToken ct)
        {
            var dist = await _uow.Reviews.GetRatingDistributionAsync(request.CourseId, ct);
            return Result<Dictionary<int, int>>.Success(dist);
        }
    }
}
