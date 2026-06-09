

namespace Horizon.Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? IconUrl { get; set; }
        public string? Color { get; set; }
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();
        public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();
        public bool IsFeatured { get; set; } = false;
        public int DisplayOrder { get; set; } = 0;
    }
}
