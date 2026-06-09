

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using MediatR;

namespace Horizon.Application.Features.Assignments.CreateAssignment
{
    public class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, Result<AssignmentDto>>
    {
        private readonly IUnitOfWork _uow;

        public CreateAssignmentHandler(IUnitOfWork uow) => _uow = uow;

        public async Task<Result<AssignmentDto>> Handle(CreateAssignmentCommand request, CancellationToken ct)
        {
            var lesson = await _uow.Lessons.GetByIdAsync(request.Dto.LessonId, ct);
            if (lesson == null)
                return Result<AssignmentDto>.NotFound("Lesson not found.");

            var section = await _uow.Sections.GetByIdAsync(lesson.SectionId, ct);
            if (section == null)
                return Result<AssignmentDto>.NotFound("Section not found.");

            var course = await _uow.Courses.GetByIdAsync(section.CourseId, ct);
            if (course == null)
                return Result<AssignmentDto>.NotFound("Course not found.");

            if (course.InstructorId != request.InstructorId)
                return Result<AssignmentDto>.Forbidden("You do not own this course.");

            var assignment = new Assignment
            {
                LessonId = request.Dto.LessonId,
                Title = request.Dto.Title,
                Description = request.Dto.Description,
                Instructions = request.Dto.Instructions,
                TotalPoints = request.Dto.TotalPoints,
                DueDate = request.Dto.DueDate,
                AllowLateSubmission = request.Dto.AllowLateSubmission,
                LatePenaltyPercentage = request.Dto.LatePenaltyPercentage,
                TimeLimitHours = request.Dto.TimeLimitHours,
            };

            await _uow.Assignments.AddAsync(assignment, ct);
            await _uow.SaveChangesAsync(ct);

            return Result<AssignmentDto>.Success(new AssignmentDto(
                assignment.Id,
                assignment.LessonId,
                assignment.Title,
                assignment.Description,
                assignment.Instructions,
                assignment.TotalPoints,
                assignment.DueDate,
                assignment.AllowLateSubmission,
                IsSubmitted: false,
                Score: null,
                IsGraded: false,
                SubmittedAt: null), 201);
        }
    }
}
