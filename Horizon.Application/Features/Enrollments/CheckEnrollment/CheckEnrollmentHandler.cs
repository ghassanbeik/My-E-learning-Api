
using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Enrollments.CheckEnrollment
{
    public class CheckEnrollmentHandler : IRequestHandler<CheckEnrollmentQuery, Result<bool>>
    {
        private readonly IUnitOfWork _uow;
        public CheckEnrollmentHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<bool>> Handle(CheckEnrollmentQuery request, CancellationToken ct)
        {
            var isEnrolled = await _uow.Enrollments.IsEnrolledAsync(request.StudentId, request.CourseId, ct);
            return Result<bool>.Success(isEnrolled);
        }
    }
}
