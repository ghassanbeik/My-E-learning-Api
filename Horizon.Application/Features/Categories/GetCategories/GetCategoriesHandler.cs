

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Categories.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;
        public GetCategoriesHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
        {
            var categories = await _cache.GetOrSetAsync(
                CacheKeys.CategoryList(),
                async () => request.IncludeSubcategories
                    ? (await _uow.Categories.GetWithSubCategoriesAsync(ct)).ToList()
                    : (await _uow.Categories.GetRootCategoriesAsync(ct)).ToList(),
                TimeSpan.FromHours(1), ct);

            List<CategoryDto> MapCategories(IEnumerable<Domain.Entities.Category> cats) =>
                cats.Select(c => new CategoryDto(c.Id, c.Name, c.Description, c.IconUrl, c.Color,
                    c.ParentId, c.IsFeatured, c.DisplayOrder,
                    c.CourseCategories.Count,
                    c.SubCategories.Any() ? MapCategories(c.SubCategories) : null)).ToList();

            return Result<List<CategoryDto>>.Success(MapCategories(categories!));
        }
    }
}
