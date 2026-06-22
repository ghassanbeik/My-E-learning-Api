using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EnrollmentEvents;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Horizon.Application.Features.Payments.ConfirmPayment
{
    public class ConfirmPaymentHandler : IRequestHandler<ConfirmPaymentCommand, Result<PaymentDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPaymentService _payment;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _config;

        public ConfirmPaymentHandler(
            IUnitOfWork uow,
            IPaymentService payment,
            IEventBus eventBus,
            IConfiguration config)
        {
            _uow = uow; _payment = payment; _eventBus = eventBus; _config = config;
        }

        public async Task<Result<PaymentDto>> Handle(ConfirmPaymentCommand request, CancellationToken ct)
        {
            // ── Ask Stripe whether the intent succeeded ────────────────────────
            var stripeResult = await _payment.ConfirmPaymentAsync(request.PaymentIntentId, ct);
            if (!stripeResult.Success)
                return Result<PaymentDto>.Failure(stripeResult.Error ?? "Payment confirmation failed.");

            // ── Idempotency guard ─────────────────────────────────────────────
            // If a Payment already exists (webhook beat the client, or the client
            // retried), return it — don't create a second Enrollment or Payment.
            var existing = await _uow.Payments.GetByTransactionIdAsync(request.PaymentIntentId, ct);
            if (existing != null)
            {
                return Result<PaymentDto>.Success(new PaymentDto(
                    existing.Id, existing.EnrollmentId, existing.TransactionId,
                    existing.PaymentMethod, existing.Amount, existing.Currency,
                    existing.Status.ToString(), existing.PaidAt, null, null));
            }

            // ── Load course and user ──────────────────────────────────────────
            var course = await _uow.Courses.GetWithDetailsAsync(request.CourseId, ct);
            if (course == null) return Result<PaymentDto>.NotFound("Course not found.");

            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            if (user == null) return Result<PaymentDto>.NotFound("User not found.");

            // ── Double-enroll guard ───────────────────────────────────────────
            if (await _uow.Enrollments.IsEnrolledAsync(request.UserId, request.CourseId, ct))
                return Result<PaymentDto>.Conflict("Already enrolled in this course.");

            // ── Platform fee from config, not hardcoded ───────────────────────
            var feePct          = decimal.Parse(_config["Stripe:PlatformFeePercentage"] ?? "15") / 100m;
            var platformFee     = Math.Round(stripeResult.Amount * feePct, 2);
            var instructorEarning = stripeResult.Amount - platformFee;

            // ── Create Enrollment first so Payment.EnrollmentId is populated ──
            var enrollment = new Enrollment
            {
                CourseId   = course.Id,
                StudentId  = request.UserId,
                Status     = EnrollmentStatus.Active,
                AmountPaid = stripeResult.Amount,
                CouponCode = stripeResult.CouponCode,     // from Stripe metadata — not client input
                EnrolledAt = DateTime.UtcNow,
                ExpiresAt  = course.IsLifetimeAccess
                                ? null
                                : DateTime.UtcNow.AddDays(course.AccessDays ?? 365),
            };
            await _uow.Enrollments.AddAsync(enrollment, ct);

            // ── Create Payment linked to the Enrollment ───────────────────────
            var payment = new Payment
            {
                EnrollmentId       = enrollment.Id,       // ← the previously-missing link
                UserId             = request.UserId,
                TransactionId      = stripeResult.TransactionId!,
                PaymentMethod      = "Stripe",
                Amount             = stripeResult.Amount,
                Currency           = course.Currency,
                Status             = PaymentStatus.Completed,
                PaidAt             = DateTime.UtcNow,
                PlatformFee        = platformFee,
                InstructorEarnings = instructorEarning,
            };
            await _uow.Payments.AddAsync(payment, ct);

            // ── Record coupon usage (code from Stripe metadata) ───────────────
            if (!string.IsNullOrEmpty(stripeResult.CouponCode))
            {
                var coupon = await _uow.Coupons.GetByCodeAsync(stripeResult.CouponCode, ct);
                if (coupon != null)
                    await _uow.Coupons.IncrementUsageAsync(coupon.Id, ct);
            }

            // ── Increment course total students ───────────────────────────────
            await _uow.Courses.IncrementStudentCountAsync(course.Id, ct);

            // ── Single atomic save ────────────────────────────────────────────
            await _uow.SaveChangesAsync(ct);

            // ── Domain events ─────────────────────────────────────────────────
            await _eventBus.PublishAsync(new PaymentCompletedEvent
            {
                PaymentId     = payment.Id,
                UserId        = request.UserId,
                CourseId      = request.CourseId,
                InstructorId  = course.InstructorId,
                UserEmail     = user.Email,
                UserName      = user.FullName,
                CourseTitle   = course.Title,
                TransactionId = stripeResult.TransactionId!,
                Amount        = stripeResult.Amount,
                Currency      = course.Currency,
                PaymentMethod = "Stripe",
            }, ct);

            await _eventBus.PublishAsync(new StudentEnrolledEvent
            {
                EnrollmentId = enrollment.Id,
                StudentId    = request.UserId,
                CourseId     = course.Id,
                InstructorId = course.InstructorId,
                StudentEmail = user.Email,
                StudentName  = user.FullName,
                CourseTitle  = course.Title,
                AmountPaid   = stripeResult.Amount,
            }, ct);

            return Result<PaymentDto>.Success(new PaymentDto(
                payment.Id, enrollment.Id, payment.TransactionId, payment.PaymentMethod,
                payment.Amount, payment.Currency, payment.Status.ToString(),
                payment.PaidAt, null, null));
        }
    }
}
