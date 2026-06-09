

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Sections.CreateSection
{
    public record CreateSectionCommand(Guid CourseId, Guid InstructorId, CreateSectionDto Dto) : IRequest<Result<SectionDto>>;

}
