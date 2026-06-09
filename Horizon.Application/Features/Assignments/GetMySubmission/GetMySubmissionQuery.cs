

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Assignments.GetMySubmission
{
    public record GetMySubmissionQuery(Guid AssignmentId, Guid StudentId) : IRequest<Result<AssignmentSubmissionDto>>;

}
