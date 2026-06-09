

namespace Horizon.Application.DTOs
{
    public record AnnouncementDto(
         Guid Id,
         Guid CourseId,
         string CourseTitle,
         Guid InstructorId,
         string InstructorName,
         string Title,
         string Content,
         bool IsPinned,
         bool IsRead,
         DateTime CreatedAt);

    public record CreateAnnouncementDto(
        Guid CourseId,
        string Title,
        string Content,
        bool IsPinned);

}
