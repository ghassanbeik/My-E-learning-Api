

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Sections.CreateSection
{
    public class CreateSectionHandler : IRequestHandler<CreateSectionCommand, Result<SectionDto>>
    {
        private readonly IUnitOfWork _uow;
        public CreateSectionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<SectionDto>> Handle(CreateSectionCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result<SectionDto>.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result<SectionDto>.Forbidden();

            var section = new Section
            {
                CourseId = request.CourseId,
                Title = request.Dto.Title,
                Description = request.Dto.Description,
                DisplayOrder = request.Dto.DisplayOrder,
            };

            await _uow.Sections.AddAsync(section, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<SectionDto>.Success(new SectionDto(
                section.Id, section.CourseId, section.Title, section.Description,
                section.DisplayOrder, section.DurationMinutes, 0, new()), 201);
        }
    }
}
