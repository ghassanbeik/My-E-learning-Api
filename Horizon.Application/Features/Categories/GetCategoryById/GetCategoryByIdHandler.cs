

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Categories.GetCategoryById
{
    public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        public GetCategoryByIdHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken ct)
        {
            var c = await _uow.Categories.GetWithCoursesAsync(request.CategoryId, ct);
            if (c == null) return Result<CategoryDto>.NotFound("Category not found.");

            return Result<CategoryDto>.Success(new CategoryDto(
                c.Id, c.Name, c.Description, c.IconUrl, c.Color, c.ParentId,
                c.IsFeatured, c.DisplayOrder, c.CourseCategories?.Count ?? 0, null));
        }
    }
}
