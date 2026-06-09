

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Sections.DeleteSection
{
    public record DeleteSectionCommand(Guid SectionId, Guid InstructorId) : IRequest<Result>;

}
