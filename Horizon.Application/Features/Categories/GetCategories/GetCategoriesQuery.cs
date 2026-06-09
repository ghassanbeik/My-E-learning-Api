

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Categories.GetCategories
{
    public record GetCategoriesQuery(bool IncludeSubcategories = true) : IRequest<Result<List<CategoryDto>>>;
}
