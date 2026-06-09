

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.GetLessonNotes
{
    public record GetLessonNotesQuery(Guid LessonId, Guid UserId) : IRequest<Result<List<LessonNoteDto>>>;

}
