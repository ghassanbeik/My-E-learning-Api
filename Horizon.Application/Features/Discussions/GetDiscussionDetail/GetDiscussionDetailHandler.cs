

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.GetDiscussionDetail
{
    public class GetDiscussionDetailHandler : IRequestHandler<GetDiscussionDetailQuery, Result<DiscussionDto>>
    {
        private readonly IUnitOfWork _uow;

        public GetDiscussionDetailHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<DiscussionDto>> Handle(GetDiscussionDetailQuery request, CancellationToken ct)
        {
            var d = await _uow.Discussions.GetWithRepliesAsync(request.DiscussionId, ct);
            if (d == null) return Result<DiscussionDto>.NotFound("Discussion not found.");

            var course = await _uow.Courses.GetByIdAsync(d.CourseId, ct);

            List<DiscussionReplyDto> MapReplies(IEnumerable<DiscussionReply> replies) =>
                replies.Select(r => new DiscussionReplyDto(
                    r.Id, r.DiscussionId, r.UserId, r.User.FullName, r.User.AvatarUrl,
                    r.Content, r.ParentReplyId, r.IsInstructorAnswer, r.UpvoteCount,
                    MapReplies(r.ChildReplies), r.CreatedAt)).ToList();

            return Result<DiscussionDto>.Success(new DiscussionDto(
                d.Id, d.CourseId, course?.Title ?? string.Empty, d.LessonId, null,
                d.UserId, d.User.FullName, d.User.AvatarUrl, d.Type.ToString(),
                d.Title, d.Content, d.IsPinned, d.IsAnswered, d.UpvoteCount, d.ReplyCount,
                MapReplies(d.Replies.Where(r => r.ParentReplyId == null)), d.CreatedAt));
        }
    }
}
