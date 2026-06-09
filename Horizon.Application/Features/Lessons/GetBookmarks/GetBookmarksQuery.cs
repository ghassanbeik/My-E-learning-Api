

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.GetBookmarks
{
    public record GetBookmarksQuery(Guid CourseId, Guid UserId) : IRequest<Result<List<LessonBookmarkDto>>>;

}
