

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Sections.UpdateSection
{
    public class UpdateSectionHandler : IRequestHandler<UpdateSectionCommand, Result<SectionDto>>
    {
        private readonly IUnitOfWork _uow;
        public UpdateSectionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<SectionDto>> Handle(UpdateSectionCommand request, CancellationToken ct)
        {
            var section = await _uow.Sections.GetWithLessonsAsync(request.SectionId, ct);
            if (section == null) return Result<SectionDto>.NotFound("Section not found.");

            var course = await _uow.Courses.GetByIdAsync(section.CourseId, ct);
            if (course?.InstructorId != request.InstructorId) return Result<SectionDto>.Forbidden();

            if (request.Dto.Title != null) section.Title = request.Dto.Title;
            if (request.Dto.Description != null) section.Description = request.Dto.Description;
            if (request.Dto.DisplayOrder != null) section.DisplayOrder = request.Dto.DisplayOrder.Value;

            await _uow.Sections.UpdateAsync(section);
            await _uow.SaveChangesAsync(ct);

            return Result<SectionDto>.Success(new SectionDto(
                section.Id, section.CourseId, section.Title, section.Description,
                section.DisplayOrder, section.DurationMinutes, section.Lessons.Count, new()));
        }
    }
}
