

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Lessons.DeleteLesson
{
    public record DeleteLessonCommand(Guid LessonId, Guid InstructorId) : IRequest<Result>;

}
