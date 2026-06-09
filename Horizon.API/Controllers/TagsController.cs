using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Categories.CreateTag;
using Horizon.Application.Features.Categories.GetTags;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/tags")]
    public class TagsController : BaseController
    {
        private readonly IMediator _mediator;
        public TagsController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get popular or search tags</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<TagDto>>), 200)]
        public async Task<IActionResult> GetTags([FromQuery] string? query, [FromQuery] int count = 20, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetTagsQuery(query, count), ct));

        /// <summary>Create a tag (Admin)</summary>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<TagDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateTagDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateTagCommand(dto), ct));
    }

}
