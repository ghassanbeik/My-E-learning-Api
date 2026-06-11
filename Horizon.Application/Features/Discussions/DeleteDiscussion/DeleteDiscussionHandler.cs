

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.DeleteDiscussion
{
    public class DeleteDiscussionHandler : IRequestHandler<DeleteDiscussionCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteDiscussionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteDiscussionCommand request, CancellationToken ct)
        {
            var discussion = await _uow.Discussions.GetByIdAsync(request.DiscussionId, ct);
            if (discussion == null) return Result.NotFound("Discussion not found.");

            var roles = (await _uow.UserRoles.GetUserRoleNamesAsync(request.UserId, ct)).ToList();
            var isAdmin = roles.Contains("Admin");
            if (discussion.UserId != request.UserId && !isAdmin) return Result.Forbidden();

            await _uow.Discussions.DeleteAsync(discussion);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
