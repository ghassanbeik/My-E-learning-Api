

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.AddLessonNote
{
    public class AddLessonNoteHandler : IRequestHandler<AddLessonNoteCommand, Result<LessonNoteDto>>
    {
        private readonly IUnitOfWork _uow;
        public AddLessonNoteHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<LessonNoteDto>> Handle(AddLessonNoteCommand request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(request.LessonId, ct);
            if (lesson == null) return Result<LessonNoteDto>.NotFound("Lesson not found.");

            var note = new LessonNote
            {
                LessonId = request.LessonId,
                UserId = request.UserId,
                Content = request.Dto.Content,
                VideoTimestampSeconds = request.Dto.VideoTimestampSeconds,
            };

            await _uow.LessonNotes.AddAsync(note, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<LessonNoteDto>.Success(new LessonNoteDto(
                note.Id, note.LessonId, note.Content, note.VideoTimestampSeconds, note.CreatedAt), 201);
        }
    }
}
