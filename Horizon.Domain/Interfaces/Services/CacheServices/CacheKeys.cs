namespace Horizon.Domain.Interfaces.Services.CacheServices
{
    public static class CacheKeys
    {
        public static string Course(Guid id) => $"course:{id}";
        public static string CourseList(string filter) => $"courses:{filter}";
        public static string Category(Guid id) => $"category:{id}";
        public static string CategoryList() => "categories:all";
        public static string User(Guid id) => $"user:{id}";
        public static string UserRoles(Guid id) => $"user:{id}:roles";
        public static string Enrollment(Guid id) => $"enrollment:{id}";
        public static string FeaturedCourses() => "courses:featured";
        public static string TopRatedCourses() => "courses:top-rated";
        public static string PopularTags() => "tags:popular";
        public static string CourseAnalytics(Guid id) => $"analytics:course:{id}";
        public static string UnreadCount(Guid userId) => $"notifications:{userId}:unread-count";
    }
}
