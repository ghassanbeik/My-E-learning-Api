using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Search.GetSearchSuggestions;
using Horizon.Application.Features.Search.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Horizon.API.Controllers
{
    [Route("api/search")]
[EnableRateLimiting("search")]
    public class SearchController : BaseController
    {
        private readonly IMediator _mediator;
        public SearchController(IMediator mediator) => _mediator = mediator;

        /// <summary>Search courses and instructors</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<SearchResponseDto>), 200)]
        public async Task<IActionResult> Search(
            [FromQuery] string query,
            [FromQuery] string category = "All",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken ct = default)
        {
            Guid? userId = CurrentUser.IsAuthenticated ? UserId : null;
            return FromResult(await _mediator.Send(new SearchQuery(query, category, page, pageSize, userId), ct));
        }

        /// <summary>Get search suggestions</summary>
        [HttpGet("suggestions")]
        [ProducesResponseType(typeof(ApiResponse<List<string>>), 200)]
        public async Task<IActionResult> Suggestions([FromQuery] string query, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetSearchSuggestionsQuery(query), ct));
    }

}
