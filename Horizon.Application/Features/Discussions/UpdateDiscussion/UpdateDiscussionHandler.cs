
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.UpdateDiscussion
{
    public class UpdateDiscussionHandler : IRequestHandler<UpdateDiscussionCommand, Result<DiscussionDto>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateDiscussionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<DiscussionDto>> Handle(UpdateDiscussionCommand request, CancellationToken ct)
        {
            var discussion = await _uow.Discussions.GetByIdAsync(request.DiscussionId, ct);
            if (discussion == null) return Result<DiscussionDto>.NotFound("Discussion not found.");
            if (discussion.UserId != request.UserId) return Result<DiscussionDto>.Forbidden();

            if (request.Dto.Title != null) discussion.Title = request.Dto.Title;
            if (request.Dto.Content != null) discussion.Content = request.Dto.Content;

            await _uow.Discussions.UpdateAsync(discussion);
            await _uow.SaveChangesAsync(ct);

            var user = await _uow.Users.GetByIdAsync(discussion.UserId, ct);
            var course = await _uow.Courses.GetByIdAsync(discussion.CourseId, ct);

            return Result<DiscussionDto>.Success(new DiscussionDto(
                discussion.Id, discussion.CourseId, course?.Title ?? string.Empty,
                discussion.LessonId, null, discussion.UserId,
                user?.FullName ?? string.Empty, user?.AvatarUrl,
                discussion.Type.ToString(), discussion.Title, discussion.Content,
                discussion.IsPinned, discussion.IsAnswered, discussion.UpvoteCount,
                discussion.ReplyCount, new(), discussion.CreatedAt));
        }
    }
}
