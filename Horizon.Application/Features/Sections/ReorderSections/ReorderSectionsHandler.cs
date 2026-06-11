

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Sections.ReorderSections
{
    public class ReorderSectionsHandler : IRequestHandler<ReorderSectionsCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        public ReorderSectionsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(ReorderSectionsCommand request, CancellationToken ct)
        {
            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result.NotFound("Course not found.");
            if (course.InstructorId != request.InstructorId) return Result.Forbidden();

            await _uow.Sections.ReorderAsync(request.CourseId, request.Orders, ct);
            await _uow.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
