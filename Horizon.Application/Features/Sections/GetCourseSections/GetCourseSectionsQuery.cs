

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Sections.GetCourseSections
{
    public record GetCourseSectionsQuery(Guid CourseId) : IRequest<Result<List<SectionDto>>>;

}
