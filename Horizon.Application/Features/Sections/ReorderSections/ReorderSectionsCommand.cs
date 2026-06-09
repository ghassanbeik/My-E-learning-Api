

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Sections.ReorderSections
{
    public record ReorderSectionsCommand(Guid CourseId, Guid InstructorId, List<(Guid SectionId, int Order)> Orders) : IRequest<Result>;

}
