

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.RejectReview
{
    public class RejectReviewHandler : IRequestHandler<RejectReviewCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public RejectReviewHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(RejectReviewCommand request, CancellationToken ct)
        {
            var review = await _uow.Reviews.GetByIdAsync(request.ReviewId, ct);
            if (review == null) return Result.NotFound("Review not found.");

            review.Status = ReviewStatus.Rejected;
            await _uow.Reviews.UpdateAsync(review);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
