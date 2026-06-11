

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Payments.GetMyPayments
{
    public class GetMyPaymentsHandler
        : IRequestHandler<GetMyPaymentsQuery, Result<PagedResponse<PaymentDto>>>
    {
        private readonly IUnitOfWork _uow;
        public GetMyPaymentsHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<PagedResponse<PaymentDto>>> Handle(
            GetMyPaymentsQuery request, CancellationToken ct)
        {
            var payments = await _uow.Payments.GetByUserAsync(request.UserId, ct);
            var items = payments.Select(p => new PaymentDto(
                p.Id, p.EnrollmentId, p.TransactionId, p.PaymentMethod,
                p.Amount, p.Currency, p.Status.ToString(),
                p.PaidAt, p.RefundAmount, p.ReceiptUrl)).ToList();

            var paged = items.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
            return Result<PagedResponse<PaymentDto>>.Success(
                PagedResponse<PaymentDto>.From(paged, items.Count, request.Page, request.PageSize));
        }
    }
}
