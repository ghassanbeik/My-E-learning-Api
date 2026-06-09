using Horizon.API.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Categories.CreateCategory;
using Horizon.Application.Features.Categories.DeleteCategory;
using Horizon.Application.Features.Categories.GetCategories;
using Horizon.Application.Features.Categories.GetCategoryById;
using Horizon.Application.Features.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers
{
    [Route("api/categories")]
    public class CategoriesController : BaseController
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator) => _mediator = mediator;

        /// <summary>Get all categories</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<CategoryDto>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] bool includeSubcategories = true, CancellationToken ct = default)
            => FromResult(await _mediator.Send(new GetCategoriesQuery(includeSubcategories), ct));

        /// <summary>Get category by ID</summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 200)]
        public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new GetCategoryByIdQuery(id), ct));

        /// <summary>Create a category (Admin)</summary>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 201)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new CreateCategoryCommand(dto), ct));

        /// <summary>Update a category (Admin)</summary>
        [HttpPut("{id:guid}")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDto>), 200)]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto, CancellationToken ct)
            => FromResult(await _mediator.Send(new UpdateCategoryCommand(id, dto), ct));

        /// <summary>Delete a category (Admin)</summary>
        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "Admin")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
            => FromResult(await _mediator.Send(new DeleteCategoryCommand(id), ct));
    }

}
