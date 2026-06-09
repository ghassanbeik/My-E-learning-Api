

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Payments.CreatePaymentIntent
{
    public record CreatePaymentIntentCommand(Guid UserId, CreatePaymentIntentDto Dto) : IRequest<Result<PaymentIntentResponseDto>>;
}
