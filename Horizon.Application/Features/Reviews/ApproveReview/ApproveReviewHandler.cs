

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.ReviewEvents;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.ApproveReview
{
    public class ApproveReviewHandler : IRequestHandler<ApproveReviewCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public ApproveReviewHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result> Handle(ApproveReviewCommand request, CancellationToken ct)
        {
            var review = await _uow.Reviews.GetByIdAsync(request.ReviewId, ct);
            if (review == null) return Result.NotFound("Review not found.");

            review.Status = ReviewStatus.Approved;
            await _uow.Reviews.UpdateAsync(review);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new ReviewApprovedEvent
            {
                ReviewId = review.Id,
                CourseId = review.CourseId,
            }, ct);

            return Result.Success();
        }
    }
}
