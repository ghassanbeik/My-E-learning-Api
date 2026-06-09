

using Horizon.Application.Common;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Reviews.VoteReview
{
    public class VoteReviewHandler : IRequestHandler<VoteReviewCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public VoteReviewHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(VoteReviewCommand request, CancellationToken ct)
        {
            var review = await _uow.Reviews.GetByIdAsync(request.ReviewId, ct);
            if (review == null) return Result.NotFound("Review not found.");

            var existing = await _uow.ReviewVotes.FirstOrDefaultAsync(
                v => v.ReviewId == request.ReviewId && v.UserId == request.UserId, ct);

            if (existing != null)
            {
                existing.IsHelpful = request.IsHelpful;
                await _uow.ReviewVotes.UpdateAsync(existing);
            }
            else
            {
                await _uow.ReviewVotes.AddAsync(new ReviewVote
                {
                    ReviewId = request.ReviewId,
                    UserId = request.UserId,
                    IsHelpful = request.IsHelpful,
                }, ct);
                if (request.IsHelpful) review.HelpfulCount++;
                await _uow.Reviews.UpdateAsync(review);
            }

            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }

}
