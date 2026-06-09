

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.ConfirmPayment
{
    public record ConfirmPaymentCommand(Guid UserId, Guid CourseId, string PaymentIntentId) : IRequest<Result<PaymentDto>>;

}
