

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Discussions.GetCourseDiscussions
{
    public class GetCourseDiscussionsHandler : IRequestHandler<GetCourseDiscussionsQuery, Result<PagedResponse<DiscussionDto>>>
    {
        private readonly IUnitOfWork _uow;

        public GetCourseDiscussionsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<DiscussionDto>>> Handle(GetCourseDiscussionsQuery request, CancellationToken ct)
        {
            DiscussionType? type = request.Type != null ? Enum.Parse<DiscussionType>(request.Type) : null;
            var result = await _uow.Discussions.GetByCourseAsync(request.CourseId, request.Page, request.PageSize, type, ct);

            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            var items = result.Items.Select(d => new DiscussionDto(
                d.Id, d.CourseId, course?.Title ?? string.Empty, d.LessonId, null,
                d.UserId, d.User.FullName, d.User.AvatarUrl, d.Type.ToString(),
                d.Title, d.Content, d.IsPinned, d.IsAnswered, d.UpvoteCount, d.ReplyCount, new(), d.CreatedAt));

            return Result<PagedResponse<DiscussionDto>>.Success(
                PagedResponse<DiscussionDto>.From(items, result.TotalCount, result.PageSize, result.PageSize));
        }
    }

}
