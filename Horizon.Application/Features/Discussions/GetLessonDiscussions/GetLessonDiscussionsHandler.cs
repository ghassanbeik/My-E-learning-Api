

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.GetLessonDiscussions
{
    public class GetLessonDiscussionsHandler
        : IRequestHandler<GetLessonDiscussionsQuery, Result<PagedResponse<DiscussionDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetLessonDiscussionsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<DiscussionDto>>> Handle(
            GetLessonDiscussionsQuery request, CancellationToken ct)
        {
            var result = await _uow.Discussions
                .GetByLessonAsync(request.LessonId, request.Page, request.PageSize, ct);

            var items = result.Items.Select(d => new DiscussionDto(
                d.Id, d.CourseId, string.Empty, d.LessonId, null,
                d.UserId, d.User?.FullName ?? string.Empty, d.User?.AvatarUrl,
                d.Type.ToString(), d.Title, d.Content, d.IsPinned, d.IsAnswered,
                d.UpvoteCount, d.ReplyCount, new(), d.CreatedAt));

            return Result<PagedResponse<DiscussionDto>>.Success(
                PagedResponse<DiscussionDto>.From(items, result.TotalCount, result.PageSize, result.PageSize));
        }
    }
}
