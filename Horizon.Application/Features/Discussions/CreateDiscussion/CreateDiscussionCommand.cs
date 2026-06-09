

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Discussions.CreateDiscussion
{
    public record CreateDiscussionCommand(Guid UserId, CreateDiscussionDto Dto) : IRequest<Result<DiscussionDto>>;

}
