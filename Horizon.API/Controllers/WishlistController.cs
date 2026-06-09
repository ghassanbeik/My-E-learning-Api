using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.WishlistCart.AddToWishlist;
using Horizon.Application.Features.WishlistCart.GetWishlis;
using Horizon.Application.Features.WishlistCart.MoveToCart;
using Horizon.Application.Features.WishlistCart.RemoveFromWishlist;
using Horizon.Infrastructure.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/wishlist")]
    public class WishlistController : BaseController
    {
        private readonly IMediator _mediator;
        public WishlistController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get my wishlist</summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<List<WishlistDto>>), 200)]
        public async Task<IActionResult> Get(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetWishlistQuery(UserId), ct));

        /// <summary>Add course to wishlist</summary>
        [HttpPost("{courseId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse), 409)]
        public async Task<IActionResult> Add(Guid courseId, CancellationToken ct)
            => FromResult(await _mediator.Send(new AddToWishlistCommand(UserId, courseId), ct));

        /// <summary>Remove course from wishlist</summary>
        [HttpDelete("{courseId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Remove(Guid courseId, CancellationToken ct)
            => FromResult(await _mediator.Send(new RemoveFromWishlistCommand(UserId, courseId), ct));

        /// <summary>Move from wishlist to cart</summary>
        [HttpPost("{courseId:guid}/move-to-cart")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CartItemDto>), 200)]
        public async Task<IActionResult> MoveToCart(Guid courseId, CancellationToken ct)
            => FromResult(await _mediator.Send(new MoveToCartCommand(UserId, courseId), ct));
    }

}
