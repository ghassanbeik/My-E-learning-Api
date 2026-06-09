

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Categories.GetCategoryById
{
    public record GetCategoryByIdQuery(Guid CategoryId) : IRequest<Result<CategoryDto>>;

}
