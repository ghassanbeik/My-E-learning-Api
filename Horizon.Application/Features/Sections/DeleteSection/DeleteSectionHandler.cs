
using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Sections.DeleteSection
{
    public class DeleteSectionHandler : IRequestHandler<DeleteSectionCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public DeleteSectionHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(DeleteSectionCommand request, CancellationToken ct)
        {
            var section = await _uow.Sections.GetByIdAsync(request.SectionId, ct);
            if (section == null) return Result.NotFound("Section not found.");

            var course = await _uow.Courses.GetByIdAsync(section.CourseId, ct);
            if (course?.InstructorId != request.InstructorId) return Result.Forbidden();

            await _uow.Sections.DeleteAsync(section);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
