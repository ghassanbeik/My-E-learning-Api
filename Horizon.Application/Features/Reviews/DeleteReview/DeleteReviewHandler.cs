

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.DeleteReview
{
    public class DeleteReviewHandler : IRequestHandler<DeleteReviewCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteReviewHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteReviewCommand request, CancellationToken ct)
        {
            var review = await _uow.Reviews.GetByIdAsync(request.ReviewId, ct);
            if (review == null) return Result.NotFound("Review not found.");
            if (review.StudentId != request.StudentId) return Result.Forbidden();

            await _uow.Reviews.DeleteAsync(review);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
