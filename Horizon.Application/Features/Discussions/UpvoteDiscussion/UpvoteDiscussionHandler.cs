

using Horizon.Application.Common;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.UpvoteDiscussion
{
    public class UpvoteDiscussionHandler : IRequestHandler<UpvoteDiscussionCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public UpvoteDiscussionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(UpvoteDiscussionCommand request, CancellationToken ct)
        {
            var discussion = await _uow.Discussions.GetByIdAsync(request.DiscussionId, ct);
            if (discussion == null) return Result.NotFound("Discussion not found.");

            var existing = await _uow.DiscussionVotes.FirstOrDefaultAsync(
                v => v.DiscussionId == request.DiscussionId && v.UserId == request.UserId, ct);

            if (existing != null) return Result.Failure("Already upvoted.");

            await _uow.DiscussionVotes.AddAsync(new DiscussionVote
            {
                DiscussionId = request.DiscussionId,
                UserId = request.UserId,
                IsUpvote = true,
            }, ct);

            await _uow.Discussions.IncrementUpvoteAsync(request.DiscussionId, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }

}
