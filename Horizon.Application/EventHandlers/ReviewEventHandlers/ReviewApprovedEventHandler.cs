

using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.ReviewEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;

namespace Horizon.Application.EventHandlers.ReviewEventHandlers
{
    public class ReviewApprovedEventHandler : IDomainEventHandler<ReviewApprovedEvent>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public ReviewApprovedEventHandler(IUnitOfWork uow, ICacheService cache)
        {
            _uow = uow;
            _cache = cache;
        }

        public async Task HandleAsync(ReviewApprovedEvent e, CancellationToken ct = default)
        {
            // Recalculate course rating
            var avgRating = await _uow.Reviews.GetAverageRatingAsync(e.CourseId, ct);
            var totalReviews = await _uow.Reviews.CountAsync(r => r.CourseId == e.CourseId &&
                                                                   r.Status == Domain.Enums.ReviewStatus.Approved, ct);
            await _uow.Courses.UpdateRatingAsync(e.CourseId, avgRating, totalReviews, ct);
            await _uow.SaveChangesAsync(ct);

            await _cache.RemoveAsync(CacheKeys.Course(e.CourseId), ct);
        }
    }
}
