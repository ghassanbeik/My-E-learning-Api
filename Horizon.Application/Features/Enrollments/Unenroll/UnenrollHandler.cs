

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Enrollments.Unenroll
{
    public class UnenrollHandler : IRequestHandler<UnenrollCommand, Result>
    {
        private readonly IUnitOfWork _uow;

        public UnenrollHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result> Handle(UnenrollCommand request, CancellationToken ct)
        {
            var enrollment = await _uow.Enrollments
                .GetByStudentAndCourseAsync(request.StudentId, request.CourseId, ct);

            if (enrollment == null)
                return Result.NotFound("Enrollment not found.");

            if (enrollment.Status == EnrollmentStatus.Completed)
                return Result.Failure("Cannot unenroll from a completed course.");

            if (enrollment.AmountPaid > 0)
                return Result.Failure(
                    "Paid enrollments must go through the refund process. Please submit a refund request.");

            enrollment.Status = EnrollmentStatus.Refunded;
            await _uow.Enrollments.UpdateAsync(enrollment);
            await _uow.Courses.DecrementStudentCountAsync(request.CourseId, ct);
            await _uow.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
