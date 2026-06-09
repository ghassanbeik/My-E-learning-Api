

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.WishlistCart.MoveToCart
{
    public record MoveToCartCommand(Guid UserId, Guid CourseId) : IRequest<Result<CartItemDto>>;

}
