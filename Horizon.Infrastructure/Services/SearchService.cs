

using Horizon.Domain.Entities;
using Horizon.Domain.Interfaces;
using Horizon.Domain.Interfaces.Services.SearchServices;
using Horizon.Domain.Repositories;

namespace Horizon.Infrastructure.Services
{
    public class SearchService : ISearchService
    {
        private readonly IUnitOfWork _uow;

        public SearchService(IUnitOfWork uow) => _uow = uow;

        public async Task<SearchResult> SearchAsync(SearchRequest request, CancellationToken ct = default)
        {
            var courses = new List<CourseSearchItem>();
            var instructors = new List<InstructorSearchItem>();

            if (request.Category is SearchCategory.All or SearchCategory.Courses)
            {
                var results = await _uow.Courses.SearchCoursesAsync(new CourseSearchParams
                {
                    Query = request.Query,
                    Page = request.Page,
                    PageSize = request.PageSize,
                }, ct);

                courses = results.Items.Select(c => new CourseSearchItem
                {
                    Id = c.Id,
                    Title = c.Title,
                    InstructorName = c.Instructor.FullName,
                    ThumbnailUrl = c.ThumbnailUrl,
                    Price = c.CurrentPrice,
                    Rating = c.AverageRating,
                    TotalStudents = c.TotalStudents,
                }).ToList();
            }

            if (request.Category is SearchCategory.All or SearchCategory.Instructors)
            {
                var results = await _uow.Users.SearchUsersAsync(request.Query, ct);
                instructors = results
                    .Where(u => u.InstructorProfiles.Any())
                    .Select(u => new InstructorSearchItem
                    {
                        Id = u.Id,
                        FullName = u.FullName,
                        AvatarUrl = u.AvatarUrl,
                        Headline = u.Headline,
                        TotalStudents = u.InstructorProfiles.FirstOrDefault()?.TotalStudents ?? 0,
                        AverageRating = u.InstructorProfiles.FirstOrDefault()?.AverageRating ?? 0,
                    }).ToList();
            }

            await LogSearchAsync(request.Query, request.UserId, courses.Count + instructors.Count, ct);

            return new SearchResult
            {
                Courses = courses,
                Instructors = instructors,
                TotalCourses = courses.Count,
                TotalInstructors = instructors.Count,
                Query = request.Query,
            };
        }

        public async Task<IEnumerable<string>> GetSuggestionsAsync(string query, CancellationToken ct = default)
        {
            var popular = await _uow.SearchLogs.GetPopularQueriesAsync(10, ct);
            return popular.Where(q => q.Contains(query, StringComparison.OrdinalIgnoreCase)).Take(5);
        }

        public async Task LogSearchAsync(string query, Guid? userId, int resultsCount, CancellationToken ct = default)
        {
            await _uow.SearchLogs.AddAsync(new SearchLog
            {
                Query = query,
                UserId = userId,
                ResultsCount = resultsCount,
                SearchedAt = DateTime.UtcNow,
            }, ct);
            await _uow.SaveChangesAsync(ct);
        }
    }

}
