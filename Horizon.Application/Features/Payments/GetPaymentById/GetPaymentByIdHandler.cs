
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Payments.GetPaymentById
{
    public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, Result<PaymentDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetPaymentByIdHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PaymentDto>> Handle(GetPaymentByIdQuery request, CancellationToken ct)
        {
            var payment = await _uow.Payments.GetByIdAsync(request.PaymentId, ct);
            if (payment == null) return Result<PaymentDto>.NotFound("Payment not found.");
            if (payment.UserId != request.UserId) return Result<PaymentDto>.Forbidden();

            return Result<PaymentDto>.Success(new PaymentDto(
                payment.Id, payment.EnrollmentId, payment.TransactionId, payment.PaymentMethod,
                payment.Amount, payment.Currency, payment.Status.ToString(),
                payment.PaidAt, payment.RefundAmount, payment.ReceiptUrl));
        }
    }
}
