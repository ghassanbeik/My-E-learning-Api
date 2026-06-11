

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.GetBookmarks
{
    public class GetBookmarksHandler : IRequestHandler<GetBookmarksQuery, Result<List<LessonBookmarkDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetBookmarksHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<LessonBookmarkDto>>> Handle(GetBookmarksQuery request, CancellationToken ct)
        {
            var bookmarks = await _uow.LessonBookmarks.GetByUserAndCourseAsync(request.UserId, request.CourseId, ct);
            return Result<List<LessonBookmarkDto>>.Success(bookmarks.Select(b =>
                new LessonBookmarkDto(b.Id, b.LessonId, b.Lesson?.Title ?? string.Empty,
                    b.VideoTimestampSeconds, b.Note, b.CreatedAt)).ToList());
        }
    }
}
