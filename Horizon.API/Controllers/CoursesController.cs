using Horizon.API.Common;
using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Application.Features.Courses.ApproveCourse;
using Horizon.Application.Features.Courses.CreateCourse;
using Horizon.Application.Features.Courses.DeleteCourse;
using Horizon.Application.Features.Courses.GetCourseById;
using Horizon.Application.Features.Courses.GetCourses;
using Horizon.Application.Features.Courses.GetCoursesByCategory;
using Horizon.Application.Features.Courses.GetFeaturedCourses;
using Horizon.Application.Features.Courses.GetInstructorCourses;
using Horizon.Application.Features.Courses.GetTopRatedCourses;
using Horizon.Application.Features.Courses.PublishCourse;
using Horizon.Application.Features.Courses.RejectCourse;
using Horizon.Application.Features.Courses.UpdateCourse;
using Horizon.Application.Features.Courses.UploadThumbnail;
using Horizon.Application.Features.Lessons.CreateLesson;
using Horizon.Application.Features.Lessons.DeleteLesson;
using Horizon.Application.Features.Lessons.UpdateLesson;
using Horizon.Application.Features.Sections.CreateSection;
using Horizon.Application.Features.Sections.DeleteSection;
using Horizon.Application.Features.Sections.GetCourseSections;
using Horizon.Application.Features.Sections.ReorderSections;
using Horizon.Application.Features.Sections.UpdateSection;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Horizon.API.Controllers;

[Route("api/courses")]
public class CoursesController : BaseController
{
    private readonly IMediator _mediator;
    public CoursesController(IMediator mediator) => _mediator = mediator;

    /// <summary>Search and filter courses</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CourseListDto>>), 200)]
    public async Task<IActionResult> GetCourses([FromQuery] CourseSearchDto search, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetCoursesQuery(search), ct));

    /// <summary>Get featured courses</summary>
    [HttpGet("featured")]
    [ProducesResponseType(typeof(ApiResponse<List<CourseListDto>>), 200)]
    public async Task<IActionResult> GetFeatured([FromQuery] int count = 8, CancellationToken ct = default)
        => FromResult(await _mediator.Send(new GetFeaturedCoursesQuery(count), ct));

    /// <summary>Get top rated courses</summary>
    [HttpGet("top-rated")]
    [ProducesResponseType(typeof(ApiResponse<List<CourseListDto>>), 200)]
    public async Task<IActionResult> GetTopRated([FromQuery] int count = 8, CancellationToken ct = default)
        => FromResult(await _mediator.Send(new GetTopRatedCoursesQuery(count), ct));

    /// <summary>Get course by ID</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CourseDetailDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        Guid? userId = CurrentUser.IsAuthenticated ? UserId : null;
        return FromResult(await _mediator.Send(new GetCourseByIdQuery(id, userId), ct));
    }

    /// <summary>Get courses by instructor</summary>
    [HttpGet("instructor/{instructorId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<List<CourseListDto>>), 200)]
    public async Task<IActionResult> GetByInstructor(Guid instructorId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetInstructorCoursesQuery(instructorId), ct));

    /// <summary>Get courses by category</summary>
    [HttpGet("category/{categoryId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<PagedResponse<CourseListDto>>), 200)]
    public async Task<IActionResult> GetByCategory(Guid categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        => FromResult(await _mediator.Send(new GetCoursesByCategoryQuery(categoryId, page, pageSize), ct));

    /// <summary>Get my courses as instructor</summary>
    [HttpGet("my-courses")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<List<CourseListDto>>), 200)]
    public async Task<IActionResult> GetMyCourses(CancellationToken ct)
        => FromResult(await _mediator.Send(new GetInstructorCoursesQuery(UserId), ct));

    /// <summary>Create a new course</summary>
    [HttpPost]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<CourseListDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> Create([FromBody] CreateCourseDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new CreateCourseCommand(UserId, dto), ct));

    /// <summary>Update a course</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<CourseListDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 403)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCourseDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new UpdateCourseCommand(id, UserId, dto), ct));

    /// <summary>Delete a course</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(ApiResponse), 403)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteCourseCommand(id, UserId), ct));

    /// <summary>Submit course for review</summary>
    [HttpPost("{id:guid}/publish")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new PublishCourseCommand(id, UserId), ct));

    /// <summary>Approve a course (Admin)</summary>
    [HttpPost("{id:guid}/approve")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Approve(Guid id, CancellationToken ct)
        => FromResult(await _mediator.Send(new ApproveCourseCommand(id), ct));

    /// <summary>Reject a course (Admin)</summary>
    [HttpPost("{id:guid}/reject")]
    [Authorize(Policy = "Admin")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Reject(Guid id, [FromBody] string reason, CancellationToken ct)
        => FromResult(await _mediator.Send(new RejectCourseCommand(id, reason), ct));

    /// <summary>Upload course thumbnail</summary>
    [HttpPost("{id:guid}/thumbnail")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    public async Task<IActionResult> UploadThumbnail(Guid id, IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest(ApiResponse.Failure("No file uploaded."));
        return FromResult(await _mediator.Send(
            new UploadThumbnailCommand(id, UserId, file.OpenReadStream(), file.FileName, file.ContentType), ct));
    }

    // ─── Sections ─────────────────────────────────────────────────────────────

    /// <summary>Get course sections</summary>
    [HttpGet("{courseId:guid}/sections")]
    [ProducesResponseType(typeof(ApiResponse<List<SectionDto>>), 200)]
    public async Task<IActionResult> GetSections(Guid courseId, CancellationToken ct)
        => FromResult(await _mediator.Send(new GetCourseSectionsQuery(courseId), ct));

    /// <summary>Create a section</summary>
    [HttpPost("{courseId:guid}/sections")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<SectionDto>), 201)]
    public async Task<IActionResult> CreateSection(Guid courseId, [FromBody] CreateSectionDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new CreateSectionCommand(courseId, UserId, dto), ct));

    /// <summary>Update a section</summary>
    [HttpPut("{courseId:guid}/sections/{sectionId:guid}")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<SectionDto>), 200)]
    public async Task<IActionResult> UpdateSection(Guid courseId, Guid sectionId, [FromBody] UpdateSectionDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new UpdateSectionCommand(sectionId, UserId, dto), ct));

    /// <summary>Delete a section</summary>
    [HttpDelete("{courseId:guid}/sections/{sectionId:guid}")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteSection(Guid courseId, Guid sectionId, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteSectionCommand(sectionId, UserId), ct));

    /// <summary>Reorder sections</summary>
    [HttpPut("{courseId:guid}/sections/reorder")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> ReorderSections(Guid courseId, [FromBody] List<SectionOrderDto> orders, CancellationToken ct)
        => FromResult(await _mediator.Send(new ReorderSectionsCommand(courseId, UserId,
            orders.Select(o => (o.SectionId, o.Order)).ToList()), ct));

    // ─── Lessons ──────────────────────────────────────────────────────────────

    /// <summary>Create a lesson</summary>
    [HttpPost("{courseId:guid}/sections/{sectionId:guid}/lessons")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<LessonDto>), 201)]
    public async Task<IActionResult> CreateLesson(Guid sectionId, [FromBody] CreateLessonDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new CreateLessonCommand(sectionId, UserId, dto), ct));

    /// <summary>Update a lesson</summary>
    [HttpPut("{courseId:guid}/sections/{sectionId:guid}/lessons/{lessonId:guid}")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(typeof(ApiResponse<LessonDto>), 200)]
    public async Task<IActionResult> UpdateLesson(Guid lessonId, [FromBody] UpdateLessonDto dto, CancellationToken ct)
        => FromResult(await _mediator.Send(new UpdateLessonCommand(lessonId, UserId, dto), ct));

    /// <summary>Delete a lesson</summary>
    [HttpDelete("{courseId:guid}/sections/{sectionId:guid}/lessons/{lessonId:guid}")]
    [Authorize(Policy = "Instructor")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> DeleteLesson(Guid lessonId, CancellationToken ct)
        => FromResult(await _mediator.Send(new DeleteLessonCommand(lessonId, UserId), ct));
}

public record SectionOrderDto(Guid SectionId, int Order);
public record LessonOrderDto(Guid LessonId, int Order);
