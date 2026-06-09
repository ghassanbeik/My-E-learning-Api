

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.MarkDiscussionAnswered
{
    public class MarkDiscussionAnsweredHandler : IRequestHandler<MarkDiscussionAnsweredCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public MarkDiscussionAnsweredHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(MarkDiscussionAnsweredCommand request, CancellationToken ct)
        {
            var discussion = await _uow.Discussions.GetByIdAsync(request.DiscussionId, ct);
            if (discussion == null) return Result.NotFound("Discussion not found.");
            if (discussion.UserId != request.UserId) return Result.Forbidden();

            discussion.IsAnswered = true;
            discussion.AcceptedReplyId = request.ReplyId;
            await _uow.Discussions.UpdateAsync(discussion);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }

}
