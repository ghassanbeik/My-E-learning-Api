

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.DeleteReply
{
    public class DeleteReplyHandler : IRequestHandler<DeleteReplyCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteReplyHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteReplyCommand request, CancellationToken ct)
        {
            var reply = await _uow.DiscussionReplies.GetByIdAsync(request.ReplyId, ct);
            if (reply == null) return Result.NotFound("Reply not found.");

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(request.UserId, ct)).ToList();
            var isAdmin = roles.Contains("Admin");
            if (reply.UserId != request.UserId && !isAdmin) return Result.Forbidden();

            await _uow.DiscussionReplies.DeleteAsync(reply);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
