
using Horizon.Application.Common;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.UpvoteReply
{
    public class UpvoteReplyHandler : IRequestHandler<UpvoteReplyCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public UpvoteReplyHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(UpvoteReplyCommand request, CancellationToken ct)
        {
            var reply = await _uow.DiscussionReplies.GetByIdAsync(request.ReplyId, ct);
            if (reply == null) return Result.NotFound("Reply not found.");

            var existing = await _uow.DiscussionVotes
                .GetByUserAndReplyAsync(request.UserId, request.ReplyId, ct);
            if (existing != null) return Result.Conflict("Already upvoted.");

            await _uow.DiscussionVotes.AddAsync(new DiscussionVote
            {
                ReplyId = request.ReplyId,
                UserId = request.UserId,
                IsUpvote = true,
            }, ct);

            reply.UpvoteCount++;
            await _uow.DiscussionReplies.UpdateAsync(reply);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
