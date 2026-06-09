

using Horizon.Domain.Enums;
using System;
using System.Net.Mime;

namespace Horizon.Domain.Entities
{
    public class Lesson : AuditableEntity
    {
        public Guid SectionId { get; set; }
        public Section Section { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public LessonContentType ContentType { get; set; } = LessonContentType.Video;
        public int DisplayOrder { get; set; } = 0;
        public int DurationMinutes { get; set; } = 0;
        public bool IsPreview { get; set; } = false;
        public bool IsDownloadable { get; set; } = false;
        public string? VideoUrl { get; set; }
        public string? ArticleContent { get; set; }
        public string? ResourceUrl { get; set; }
        public string? ResourceType { get; set; }
        public ICollection<Progress> Progresses { get; set; } = new List<Progress>();
        public ICollection<Quiz> Quizzes { get; set; } = new List<Quiz>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<LessonNote> Notes { get; set; } = new List<LessonNote>();
        public ICollection<LessonBookmark> Bookmarks { get; set; } = new List<LessonBookmark>();
    }
}
