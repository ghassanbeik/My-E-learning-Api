

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Categories.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;
        public CreateCategoryHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken ct)
        {
            if (await _uow.Categories.NameExistsAsync(request.Dto.Name, null, ct))
                return Result<CategoryDto>.Conflict("Category name already exists.");

            var category = new Domain.Entities.Category
            {
                Name = request.Dto.Name,
                Description = request.Dto.Description,
                IconUrl = request.Dto.IconUrl,
                Color = request.Dto.Color,
                ParentId = request.Dto.ParentId,
                IsFeatured = request.Dto.IsFeatured,
                DisplayOrder = request.Dto.DisplayOrder,
            };

            await _uow.Categories.AddAsync(category, ct);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveAsync(CacheKeys.CategoryList(), ct);

            return Result<CategoryDto>.Success(new CategoryDto(
                category.Id, category.Name, category.Description, category.IconUrl,
                category.Color, category.ParentId, category.IsFeatured, category.DisplayOrder, 0, null), 201);
        }
    }
}
