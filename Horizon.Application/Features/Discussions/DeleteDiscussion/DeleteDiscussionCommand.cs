

using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Discussions.DeleteDiscussion
{
    public record DeleteDiscussionCommand(Guid DiscussionId, Guid UserId) : IRequest<Result>;

}
