
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.GetCart
{
    public record GetCartQuery(Guid UserId) : IRequest<Result<CartSummaryDto>>;

}
