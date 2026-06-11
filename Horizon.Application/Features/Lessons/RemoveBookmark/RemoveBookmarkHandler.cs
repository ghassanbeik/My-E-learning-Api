

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.RemoveBookmark
{
    public class RemoveBookmarkHandler : IRequestHandler<RemoveBookmarkCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public RemoveBookmarkHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(RemoveBookmarkCommand request, CancellationToken ct)
        {
            var bookmark = await _uow.LessonBookmarks.GetByIdAsync(request.BookmarkId, ct);
            if (bookmark == null) return Result.NotFound("Bookmark not found.");
            if (bookmark.UserId != request.UserId) return Result.Forbidden();

            await _uow.LessonBookmarks.DeleteAsync(bookmark);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
