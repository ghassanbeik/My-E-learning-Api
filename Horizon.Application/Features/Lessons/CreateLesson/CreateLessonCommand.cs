

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Lessons.CreateLesson
{
    public record CreateLessonCommand(Guid SectionId, Guid InstructorId, CreateLessonDto Dto) : IRequest<Result<LessonDto>>;
}
