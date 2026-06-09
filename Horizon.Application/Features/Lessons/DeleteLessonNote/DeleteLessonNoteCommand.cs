
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Lessons.DeleteLessonNote
{
    public record DeleteLessonNoteCommand(Guid NoteId, Guid UserId) : IRequest<Result>;

}
