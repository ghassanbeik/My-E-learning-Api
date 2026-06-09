

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Discussions.GetDiscussionDetail
{
    public record GetDiscussionDetailQuery(Guid DiscussionId) : IRequest<Result<DiscussionDto>>;

}
