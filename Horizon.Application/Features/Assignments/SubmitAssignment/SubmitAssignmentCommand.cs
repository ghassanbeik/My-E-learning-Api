

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Assignments.SubmitAssignment
{
    public record SubmitAssignmentCommand(Guid AssignmentId, Guid StudentId, SubmitAssignmentDto Dto) : IRequest<Result<AssignmentSubmissionDto>>;
}
