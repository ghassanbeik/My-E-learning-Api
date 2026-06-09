

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.GetPaymentById
{
    public record GetPaymentByIdQuery(Guid PaymentId, Guid UserId) : IRequest<Result<PaymentDto>>;

}
