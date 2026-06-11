

using Horizon.Application.Common;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.CacheServices;
using MediatR;

namespace Horizon.Application.Features.Categories.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result>
    {
        private readonly IUnitOfWork _uow;
        private readonly ICacheService _cache;

        public DeleteCategoryHandler(IUnitOfWork uow, ICacheService cache) { _uow = uow; _cache = cache; }

        public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken ct)
        {
            var category = await _uow.Categories.GetByIdAsync(request.CategoryId, ct);
            if (category == null) return Result.NotFound("Category not found.");

            var hasCourses = await _uow.CourseCategories.ExistsAsync(request.CategoryId , ct);
            if (hasCourses)
                return Result.Failure("Cannot delete a category that has courses assigned to it.");

            await _uow.Categories.DeleteAsync(category);
            await _uow.SaveChangesAsync(ct);
            await _cache.RemoveAsync(CacheKeys.CategoryList(), ct);
            return Result.Success();
        }
    }
}
