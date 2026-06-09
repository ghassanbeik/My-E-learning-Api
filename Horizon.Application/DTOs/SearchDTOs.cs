

namespace Horizon.Application.DTOs
{
    public record SearchResponseDto(
       string Query,
       List<CourseListDto> Courses,
       List<InstructorDto> Instructors,
       int TotalCourses,
       int TotalInstructors);

}
