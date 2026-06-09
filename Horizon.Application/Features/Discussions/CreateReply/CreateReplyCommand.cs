

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Discussions.CreateReply
{
    public record CreateReplyCommand(Guid DiscussionId, Guid UserId, CreateDiscussionReplyDto Dto) : IRequest<Result<DiscussionReplyDto>>;

}
