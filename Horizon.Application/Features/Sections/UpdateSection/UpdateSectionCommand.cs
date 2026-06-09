
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Sections.UpdateSection
{
    public record UpdateSectionCommand(Guid SectionId, Guid InstructorId, UpdateSectionDto Dto) : IRequest<Result<SectionDto>>;

}
