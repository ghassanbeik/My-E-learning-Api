

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.PinDiscussion
{
    public class PinDiscussionHandler : IRequestHandler<PinDiscussionCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public PinDiscussionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(PinDiscussionCommand request, CancellationToken ct)
        {
            var discussion = await _uow.Discussions.GetByIdAsync(request.DiscussionId, ct);
            if (discussion == null) return Result.NotFound("Discussion not found.");

            var course = await _uow.Courses.GetByIdAsync(discussion.CourseId, ct);
            if (course?.InstructorId != request.InstructorId) return Result.Forbidden();

            discussion.IsPinned = !discussion.IsPinned;
            await _uow.Discussions.UpdateAsync(discussion);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }

}
