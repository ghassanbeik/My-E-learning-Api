

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Categories.CreateCategory
{
    public record CreateCategoryCommand(CreateCategoryDto Dto) : IRequest<Result<CategoryDto>>;

}
