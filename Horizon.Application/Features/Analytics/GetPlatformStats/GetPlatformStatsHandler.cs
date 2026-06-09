

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Analytics.GetPlatformStats
{
    public class GetPlatformStatsHandler : IRequestHandler<GetPlatformStatsQuery, Result<PlatformStatsDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetPlatformStatsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PlatformStatsDto>> Handle(GetPlatformStatsQuery request, CancellationToken ct)
        {
            var totalUsers = await _uow.Users.CountAsync(x => true, ct);
            var totalInstructors = await _uow.Users.GetTotalStudentsCountAsync(ct);
            var totalCourses = await _uow.Courses.CountAsync(x => true, ct);
            var totalEnrollments = await _uow.Enrollments.CountAsync(x => true, ct);
            var totalRevenue = await _uow.Payments.GetTotalRevenueAsync(ct: ct);
            var totalCerts = await _uow.Certificates.CountAsync(x => true, ct);

            return Result<PlatformStatsDto>.Success(new PlatformStatsDto(
                totalUsers, totalInstructors, totalUsers - totalInstructors,
                totalCourses, totalEnrollments, totalRevenue, totalCerts));
        }
    }
}
