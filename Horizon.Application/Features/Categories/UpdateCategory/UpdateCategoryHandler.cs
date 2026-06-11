

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Categories.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public UpdateCategoryHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken ct)
        {
            var category = await _uow.Categories.GetByIdAsync(request.CategoryId, ct);
            if (category == null) return Result<CategoryDto>.NotFound("Category not found.");

            if (request.Dto.Name != null &&
                await _uow.Categories.NameExistsAsync(request.Dto.Name, request.CategoryId, ct))
                return Result<CategoryDto>.Conflict("Category name already exists.");

            if (request.Dto.Name != null) category.Name = request.Dto.Name;
            if (request.Dto.Description != null) category.Description = request.Dto.Description;
            if (request.Dto.IconUrl != null) category.IconUrl = request.Dto.IconUrl;
            if (request.Dto.Color != null) category.Color = request.Dto.Color;
            if (request.Dto.IsFeatured != null) category.IsFeatured = request.Dto.IsFeatured.Value;
            if (request.Dto.DisplayOrder != null) category.DisplayOrder = request.Dto.DisplayOrder.Value;

           await _uow.Categories.UpdateAsync(category);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveAsync(CacheKeys.CategoryList(), ct);

            return Result<CategoryDto>.Success(new CategoryDto(
                category.Id, category.Name, category.Description, category.IconUrl,
                category.Color, category.ParentId, category.IsFeatured, category.DisplayOrder, 0, null));
        }
    }
}
