

using Horizon.Application.Common;
using Horizon.Application.DTOs;
using Horizon.Domain.Interfaces.Services.SearchServices;
using MediatR;

namespace Horizon.Application.Features.Search.Search
{
    public class SearchHandler : IRequestHandler<SearchQuery, Result<SearchResponseDto>>
    {
        private readonly ISearchService _search;
        public SearchHandler(ISearchService search) => _search = search;

        public async Task<Result<SearchResponseDto>> Handle(SearchQuery request, CancellationToken ct)
        {
            var result = await _search.SearchAsync(new SearchRequest
            {
                Query = request.Query,
                Category = Enum.Parse<SearchCategory>(request.Category),
                Page = request.Page,
                PageSize = request.PageSize,
                UserId = request.UserId,
            }, ct);

            return Result<SearchResponseDto>.Success(new SearchResponseDto(
                result.Query,
                result.Courses.Select(c => new CourseListDto(
                    c.Id, c.Title, null, null, c.ThumbnailUrl,
                    c.InstructorName, Guid.Empty, null, string.Empty, string.Empty,
                    c.Price, null, c.Price, false, c.Rating, 0,
                    c.TotalStudents, 0, 0, false, new(), new(), DateTime.UtcNow)).ToList(),
                result.Instructors.Select(i => new InstructorDto(
                    i.Id, i.FullName, string.Empty, i.AvatarUrl, i.Headline,
                    null, i.AverageRating, i.TotalStudents, 0, 0, 0, false)).ToList(),
                result.TotalCourses,
                result.TotalInstructors));
        }
    }

}
