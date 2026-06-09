

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.AddToCart
{
    public record AddToCartCommand(Guid UserId, Guid CourseId, string? CouponCode) : IRequest<Result<CartItemDto>>;

}
