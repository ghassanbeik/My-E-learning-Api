

namespace Horizon.Application.DTOs
{
    public record DiscussionDto(
        Guid Id,
        Guid CourseId,
        string CourseTitle,
        Guid? LessonId,
        string? LessonTitle,
        Guid UserId,
        string UserName,
        string? UserAvatar,
        string Type,
        string Title,
        string Content,
        bool IsPinned,
        bool IsAnswered,
        int UpvoteCount,
        int ReplyCount,
        List<DiscussionReplyDto> Replies,
        DateTime CreatedAt);

    public record CreateDiscussionDto(
        Guid CourseId,
        Guid? LessonId,
        string Type,
        string Title,
        string Content);

    public record UpdateDiscussionDto(string? Title, string? Content);

    public record DiscussionReplyDto(
        Guid Id,
        Guid DiscussionId,
        Guid UserId,
        string UserName,
        string? UserAvatar,
        string Content,
        Guid? ParentReplyId,
        bool IsInstructorAnswer,
        int UpvoteCount,
        List<DiscussionReplyDto> ChildReplies,
        DateTime CreatedAt);

    public record CreateDiscussionReplyDto(string Content, Guid? ParentReplyId);
}
