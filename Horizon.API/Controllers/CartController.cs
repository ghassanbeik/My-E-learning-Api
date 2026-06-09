using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.WishlistCart.AddToCart;
using Horizon.Application.Features.WishlistCart.ClearCart;
using Horizon.Application.Features.WishlistCart.GetCart;
using Horizon.Application.Features.WishlistCart.RemoveFromCart;
using Horizon.Infrastructure.Seeding;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/cart")]
    public class CartController : BaseController
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get cart with summary</summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CartSummaryDto>), 200)]
        public async Task<IActionResult> Get(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetCartQuery(UserId), ct));

        /// <summary>Add course to cart</summary>
        [HttpPost("{courseId:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<CartItemDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse), 409)]
        public async Task<IActionResult> Add(Guid courseId, [FromQuery] string? couponCode, CancellationToken ct)
            => FromResult(await _mediator.Send(new AddToCartCommand(UserId, courseId, couponCode), ct));

        /// <summary>Remove course from cart</summary>
        [HttpDelete("{courseId:guid}")]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Remove(Guid courseId, CancellationToken ct)
            => FromResult(await _mediator.Send(new RemoveFromCartCommand(UserId, courseId), ct));

        /// <summary>Clear entire cart</summary>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Clear(CancellationToken ct)
            => FromResult(await _mediator.Send(new ClearCartCommand(UserId), ct));
    }
}
