

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Lessons.ReorderLessons
{
    public record ReorderLessonsCommand(Guid SectionId, Guid InstructorId, List<(Guid LessonId, int Order)> Orders) : IRequest<Result>;

}
