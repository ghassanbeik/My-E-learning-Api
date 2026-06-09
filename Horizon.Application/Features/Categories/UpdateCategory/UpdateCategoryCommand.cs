

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Categories.UpdateCategory
{
    public record UpdateCategoryCommand(Guid CategoryId, UpdateCategoryDto Dto) : IRequest<Result<CategoryDto>>;

}
