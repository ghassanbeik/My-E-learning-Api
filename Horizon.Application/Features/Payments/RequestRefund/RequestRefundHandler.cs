

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Enums;
using Horizon.Domain.Events.EventInterfaces;
using Horizon.Domain.Events.PaymentEvents;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Payments.RequestRefund
{
    public class RequestRefundHandler
         : IRequestHandler<RequestRefundCommand, Result<RefundRequestResponseDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IEventBus _eventBus;

        public RequestRefundHandler(IUnitOfWork uow, IEventBus eventBus)
        { _uow = uow; _eventBus = eventBus; }

        public async Task<Result<RefundRequestResponseDto>> Handle(
            RequestRefundCommand request, CancellationToken ct)
        {
            var payment = await _uow.Payments.GetByIdAsync(request.Dto.PaymentId, ct);
            if (payment == null) return Result<RefundRequestResponseDto>.NotFound("Payment not found.");
            if (payment.UserId != request.UserId) return Result<RefundRequestResponseDto>.Forbidden();
            if (payment.Status != PaymentStatus.Completed)
                return Result<RefundRequestResponseDto>.Failure("Only completed payments can be refunded.");

            var existing = await _uow.RefundRequests.GetByPaymentAsync(request.Dto.PaymentId, ct);
            if (existing != null)
                return Result<RefundRequestResponseDto>.Conflict("A refund request already exists for this payment.");

            var refundRequest = new RefundRequest
            {
                PaymentId = request.Dto.PaymentId,
                UserId = request.UserId,
                Reason = request.Dto.Reason,
                Status = RefundStatus.Pending,
            };

            await _uow.RefundRequests.AddAsync(refundRequest, ct);
            await _uow.SaveChangesAsync(ct);

            var user = await _uow.Users.GetByIdAsync(request.UserId, ct);
            await _eventBus.PublishAsync(new RefundRequestedEvent
            {
                RefundRequestId = refundRequest.Id,
                PaymentId = request.Dto.PaymentId,
                UserId = request.UserId,
                UserEmail = user?.Email ?? string.Empty,
                UserName = user?.FullName ?? string.Empty,
                Reason = request.Dto.Reason,
                CourseTitle = string.Empty,
            }, ct);

            return Result<RefundRequestResponseDto>.Success(new RefundRequestResponseDto(
                refundRequest.Id, refundRequest.Status.ToString(),
                refundRequest.Reason, refundRequest.CreatedAt), 201);
        }
    }
}
