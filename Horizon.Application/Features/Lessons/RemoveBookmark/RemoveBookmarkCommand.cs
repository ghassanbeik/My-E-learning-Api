

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Lessons.RemoveBookmark
{
    public record RemoveBookmarkCommand(Guid BookmarkId, Guid UserId) : IRequest<Result>;

}
