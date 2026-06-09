
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using MediatR;

namespace Horizon.Application.Features.Assignments.CreateAssignment
{
    public record CreateAssignmentCommand(Guid InstructorId, CreateAssignmentDto Dto) : IRequest<Result<AssignmentDto>>;

}
