

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.AddLessonNote
{
    public record AddLessonNoteCommand(Guid LessonId, Guid UserId, CreateLessonNoteDto Dto) : IRequest<Result<LessonNoteDto>>;

}
