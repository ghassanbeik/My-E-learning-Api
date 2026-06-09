using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Payments.CreateCoupon;
using Horizon.Application.Features.Payments.GetActiveCoupons;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/coupons")]
    public class CouponsController : BaseController
    {
        private readonly IMediator _mediator;
        public CouponsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get all active coupons (Admin)</summary>
        [HttpGet]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<List<CouponDto>>), 200)]
        public async Task<IActionResult> GetActive(CancellationToken ct)
            => FromResult(await _mediator.Send(new GetActiveCouponsQuery(), ct));

        /// <summary>Create a coupon (Admin)</summary>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<CouponDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateCouponDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateCouponCommand(dto), ct));
    }
}
