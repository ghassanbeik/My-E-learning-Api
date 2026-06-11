

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Lessons.DeleteLessonNote
{
    public class DeleteLessonNoteHandler : IRequestHandler<DeleteLessonNoteCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteLessonNoteHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteLessonNoteCommand request, CancellationToken ct)
        {
            var note = await _uow.LessonNotes.GetByIdAsync(request.NoteId, ct);
            if (note == null) return Result.NotFound("Note not found.");
            if (note.UserId != request.UserId) return Result.Forbidden();

            await _uow.LessonNotes.DeleteAsync(note);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
