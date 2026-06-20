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
    /// <summary>
    /// Direct enrollment — valid ONLY when the course price (after any coupon)
    /// is exactly zero. Paid courses MUST go through the Stripe checkout flow:
    ///   POST /api/payments/intent  →  Stripe  →  POST /api/payments/confirm
    ///
    /// ConfirmPaymentHandler creates the Enrollment atomically after Stripe
    /// confirms the charge, so this endpoint never needs to handle paid courses.
    ///
    /// WITHOUT this guard, any authenticated student can call POST /enrollments
    /// directly for a $99 course and receive full access for $0.
    /// </summary>
    public class EnrollHandler : IRequestHandler<EnrollCommand, Result<EnrollmentDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public EnrollHandler(IUnitOfWork uow, IEventBus eventBus)
        {
            _uow = uow; _eventBus = eventBus;
        }

        public async Task<Result<EnrollmentDto>> Handle(EnrollCommand request, CancellationToken ct)
        {
            if (await _uow.Enrollments.IsEnrolledAsync(request.StudentId, request.CourseId, ct))
                return Result<EnrollmentDto>.Conflict("Already enrolled in this course.");

            var course = await _uow.Courses.GetByIdAsync(request.CourseId, ct);
            if (course == null)
                return Result<EnrollmentDto>.NotFound("Course not found.");
            if (course.Status != CourseStatus.Published)
                return Result<EnrollmentDto>.Failure("Course is not available.");

            var student = await _uow.Users.GetByIdAsync(request.StudentId, ct);
            if (student == null)
                return Result<EnrollmentDto>.NotFound("Student not found.");

            // ── Coupon application ────────────────────────────────────────────
            decimal finalPrice = course.CurrentPrice;
            Guid? consumedCouponId = null;

            if (!string.IsNullOrEmpty(request.CouponCode))
            {
                var coupon = await _uow.Coupons.GetByCodeAsync(request.CouponCode, ct);
                if (coupon != null && await _uow.Coupons.IsValidAsync(request.CouponCode, course.Id, ct))
                {
                    var discount = coupon.Type == CouponType.Percentage
                        ? finalPrice * coupon.Value / 100
                        : coupon.Value;

                    if (coupon.MaxDiscountAmount.HasValue)
                        discount = Math.Min(discount, coupon.MaxDiscountAmount.Value);

                    finalPrice = Math.Max(0, finalPrice - discount);
                    consumedCouponId = coupon.Id;
                }
            }

            // ── THE CRITICAL PAID-COURSE GUARD ───────────────────────────────
            // If, after applying any coupon, the course still costs money the
            // student must go through the Stripe payment flow. This endpoint
            // only handles genuinely-free (or fully-discounted) enrollment.
            if (finalPrice > 0)
            {
                return Result<EnrollmentDto>.Failure(
                    "This course requires payment. " +
                    "Use POST /api/payments/intent to start checkout, " +
                    "then confirm the payment — you will be enrolled automatically.");
            }

            // Consume the coupon now (after the paid-course guard so we only
            // increment usage for coupons that actually enabled a free enroll).
            if (consumedCouponId.HasValue)
                await _uow.Coupons.IncrementUsageAsync(consumedCouponId.Value, ct);

            var enrollment = new Enrollment
            {
                CourseId   = course.Id,
                StudentId  = request.StudentId,
                Status     = EnrollmentStatus.Active,
                AmountPaid = 0,
                CouponCode = request.CouponCode,
                EnrolledAt = DateTime.UtcNow,
                ExpiresAt  = course.IsLifetimeAccess
                                ? null
                                : DateTime.UtcNow.AddDays(course.AccessDays ?? 365),
            };

            await _uow.Enrollments.AddAsync(enrollment, ct);
            await _uow.Courses.IncrementStudentCountAsync(course.Id, ct);
            await _uow.SaveChangesAsync(ct);

            await _eventBus.PublishAsync(new StudentEnrolledEvent
            {
                EnrollmentId = enrollment.Id,
                StudentId    = request.StudentId,
                CourseId     = course.Id,
                InstructorId = course.InstructorId,
                StudentEmail = student.Email,
                StudentName  = student.FullName,
                CourseTitle  = course.Title,
                AmountPaid   = 0,
            }, ct);

            return Result<EnrollmentDto>.Success(new EnrollmentDto(
                enrollment.Id, course.Id, course.Title, course.ThumbnailUrl,
                string.Empty, enrollment.Status.ToString(), 0, 0, 0,
                course.TotalLessons, enrollment.EnrolledAt, null, enrollment.ExpiresAt, null), 201);
        }
    }
}
