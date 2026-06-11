

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Enrollments.GetMyEnrollments
{
    public class GetMyEnrollmentsHandler
        : IRequestHandler<GetMyEnrollmentsQuery, Result<PagedResponse<EnrollmentDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetMyEnrollmentsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<EnrollmentDto>>> Handle(
            GetMyEnrollmentsQuery request, CancellationToken ct)
        {
            var enrollments = await _uow.Enrollments.GetByStudentAsync(request.StudentId, ct);
            var items = new List<EnrollmentDto>();

            foreach (var e in enrollments)
            {
                var completed = await _uow.Progresses.GetCompletedCountAsync(e.Id, ct);
                items.Add(new EnrollmentDto(
                    e.Id, e.CourseId, e.Course.Title, e.Course.ThumbnailUrl,
                    e.Course.Instructor?.FullName ?? string.Empty,
                    e.Status.ToString(), e.AmountPaid, e.ProgressPercentage,
                    completed, e.Course.TotalLessons,
                    e.EnrolledAt, e.CompletedAt, e.ExpiresAt, e.LastAccessedAt));
            }

            var paged = items
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);

            return Result<PagedResponse<EnrollmentDto>>.Success(
                PagedResponse<EnrollmentDto>.From(paged, items.Count, request.Page, request.PageSize));
        }
    }
}
