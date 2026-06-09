

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Discussions.UpdateDiscussion
{
    public record UpdateDiscussionCommand(Guid DiscussionId, Guid UserId, UpdateDiscussionDto Dto) : IRequest<Result<DiscussionDto>>;

}
