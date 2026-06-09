

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Categories.DeleteCategory
{
    public record DeleteCategoryCommand(Guid CategoryId) : IRequest<Result>;

}
