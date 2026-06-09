

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Sections.GetCourseSections
{
    public class GetCourseSectionsHandler : IRequestHandler<GetCourseSectionsQuery, Result<List<SectionDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetCourseSectionsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<List<SectionDto>>> Handle(GetCourseSectionsQuery request, CancellationToken ct)
        {
            var sections = await _uow.Sections.GetByCourseAsync(request.CourseId, ct);
            var items = sections.Select(s => new SectionDto(
                s.Id, s.CourseId, s.Title, s.Description, s.DisplayOrder,
                s.DurationMinutes, s.Lessons.Count, new())).ToList();
            return Result<List<SectionDto>>.Success(items);
        }
    }
}
