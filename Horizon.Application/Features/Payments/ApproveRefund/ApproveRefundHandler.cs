

using Horizon.Application.Common;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.PaymentStripeServices;
using MediatR;

namespace Horizon.Application.Features.Payments.ApproveRefund
{
    public class ApproveRefundHandler : IRequestHandler<ApproveRefundCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly IPaymentService _payment;
        private readonly IEventBus _eventBus;

        public ApproveRefundHandler(IUnitOfWork uow, IPaymentService payment, IEventBus eventBus)
        { _uow = uow; _payment = payment; _eventBus = eventBus; }

        public async Task<Result> Handle(ApproveRefundCommand request, CancellationToken ct)
        {
            var refundRequest = await _uow.RefundRequests.GetByIdAsync(request.RefundRequestId, ct);
            if (refundRequest == null) return Result.NotFound("Refund request not found.");
            if (refundRequest.Status != RefundStatus.Pending)
                return Result.Failure("This refund request has already been processed.");

            var payment = await _uow.Payments.GetByIdAsync(refundRequest.PaymentId, ct);
            if (payment == null) return Result.NotFound("Payment not found.");

            var refundResult = await _payment.RefundAsync(payment.TransactionId, payment.Amount, "requested_by_customer", ct);
            if (!refundResult.Success)
                return Result.Failure(refundResult.Error ?? "Refund processing failed.");

            payment.Status = PaymentStatus.Refunded;
            payment.RefundedAt = DateTime.UtcNow;
            payment.RefundAmount = refundResult.Amount;
            await _uow.Payments.UpdateAsync(payment);

            refundRequest.Status = RefundStatus.Processed;
            refundRequest.ResolvedAt = DateTime.UtcNow;
            refundRequest.ResolvedById = request.AdminId;
            await _uow.RefundRequests.UpdateAsync(refundRequest);

            if (payment.EnrollmentId.HasValue)
            {
                var enrollment = await _uow.Enrollments.GetByIdAsync(payment.EnrollmentId.Value, ct);
                if (enrollment != null)
                {
                    enrollment.Status = EnrollmentStatus.Refunded;
                    await _uow.Enrollments.UpdateAsync(enrollment);
                    await _uow.Courses.DecrementStudentCountAsync(enrollment.CourseId, ct);
                }
            }

            await _uow.SaveChangesAsync(ct);

            var user = await _uow.Users.GetByIdAsync(refundRequest.UserId, ct);
            await _eventBus.PublishAsync(new RefundApprovedEvent
            {
                RefundRequestId = refundRequest.Id,
                UserId = refundRequest.UserId,
                UserEmail = user?.Email ?? string.Empty,
                UserName = user?.FullName ?? string.Empty,
                Amount = refundResult.Amount,
                CourseTitle = string.Empty,
            }, ct);

            return Result.Success();
        }
    }
}
