
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.UpdateLesson
{
    public record UpdateLessonCommand(Guid LessonId, Guid InstructorId, UpdateLessonDto Dto) : IRequest<Result<LessonDto>>;

}
