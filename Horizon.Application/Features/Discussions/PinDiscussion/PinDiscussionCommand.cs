
using Horizon.Application.Common;
using MediatR;

namespace Horizon.Application.Features.Discussions.PinDiscussion
{
    public record PinDiscussionCommand(Guid DiscussionId, Guid InstructorId) : IRequest<Result>;

}
