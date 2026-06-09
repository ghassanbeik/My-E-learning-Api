

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.AddBookmark
{
    public class AddBookmarkHandler : IRequestHandler<AddBookmarkCommand, Result<LessonBookmarkDto>>
    {
        private readonly IUnitOfWork _uow;
        public AddBookmarkHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<LessonBookmarkDto>> Handle(AddBookmarkCommand request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(request.LessonId, ct);
            if (lesson == null) return Result<LessonBookmarkDto>.NotFound("Lesson not found.");

            var bookmark = new LessonBookmark
            {
                LessonId = request.LessonId,
                UserId = request.UserId,
                VideoTimestampSeconds = request.Dto.VideoTimestampSeconds,
                Note = request.Dto.Note,
            };

            await _uow.LessonBookmarks.AddAsync(bookmark, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<LessonBookmarkDto>.Success(new LessonBookmarkDto(
                bookmark.Id, bookmark.LessonId, lesson.Title,
                bookmark.VideoTimestampSeconds, bookmark.Note, bookmark.CreatedAt), 201);
        }
    }
}
