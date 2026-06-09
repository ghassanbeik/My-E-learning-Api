

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Enrollments.Enroll
{
    public class EnrollHandler : IRequestHandler<EnrollCommand, Result<EnrollmentDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public EnrollHandler(IUnitOfWork uow, IEventBus eventBus) { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<EnrollmentDto>> Handle(EnrollCommand request, CancellationToken ct)
        {
            if (await _uow.Enrollments.IsEnrolledAsync(request.StudentId, request.CourseId, ct))
                return Result<EnrollmentDto>.Conflict("Already enrolled in this course.");

            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null) return Result<EnrollmentDto>.NotFound("Course not found.");
            if (course.Status != CourseStatus.Published) return Result<EnrollmentDto>.Failure("Course is not available.");

            var student = await _uow.Users.GetByIdAsync(request.StudentId, ct);
            if (student == null) return Result<EnrollmentDto>.NotFound("Student not found.");

            decimal amountPaid = course.CurrentPrice;

            // Apply coupon if provided
            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                var coupon = await _uow.Coupons.GetByCodeAsync(request.CouponCode, ct);
                if (coupon != null && await _uow.Coupons.IsValidAsync(request.CouponCode, course.Id, ct))
                {
                    var discount = coupon.Type == CouponType.Percentage
                        ? amountPaid * coupon.Value / 100
                        : coupon.Value;

                    if (coupon.MaxDiscountAmount.HasValue)
                        discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);

                    amountPaid = Math.Max(0, amountPaid - discount);
                    await _uow.Coupons.IncrementUsageAsync(coupon.Id, ct);
                }
            }

            var enrollment = new Enrollment
            {
                CourseId = course.Id,
                StudentId = request.StudentId,
                Status = EnrollmentStatus.Active,
                AmountPaid = amountPaid,
                EnrolledAt = DateTime.UtcNow,
                ExpiresAt = course.IsLifetimeAccess ? null : DateTime.UtcNow.AddDays(course.AccessDays ?? 365),
            };

            await _uow.Enrollments.AddAsync(enrollment, ct);
            await _uow.Courses.IncrementStudentCountAsync(course.Id, ct);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new StudentEnrolledEvent
            {
                EnrollmentId = enrollment.Id,
                StudentId = request.StudentId,
                CourseId = course.Id,
                InstructorId = course.InstructorId,
                StudentEmail = student.Email,
                StudentName = student.FullName,
                CourseTitle = course.Title,
                AmountPaid = amountPaid,
            }, ct);

            return Result<EnrollmentDto>.Success(new EnrollmentDto(
                enrollment.Id, course.Id, course.Title, course.ThumbnailUrl,
                string.Empty, enrollment.Status.ToString(), amountPaid, 0, 0,
                course.TotalLessons, enrollment.EnrolledAt, null, enrollment.ExpiresAt, null), 201);
        }
    }

}
