using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Payments.HandleWebhook
{
    public record HandleWebhookCommand(string Payload,string StripeSignature) : IRequest<Result<string>>;
}
