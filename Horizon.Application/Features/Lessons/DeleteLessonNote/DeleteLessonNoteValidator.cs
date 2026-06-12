

using FluentValidation;

namespace Horizon.Application.Features.Lessons.DeleteLessonNote
{
    public class DeleteLessonNoteValidator : AbstractValidator<DeleteLessonNoteCommand>
    {
        public DeleteLessonNoteValidator()
        {
            RuleFor(x => x.NoteId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
