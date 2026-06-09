

using FluentValidation;

namespace Horizon.Application.Features.Search.Search
{
    public class SearchQueryValidator : AbstractValidator<SearchQuery>
    {
        public SearchQueryValidator()
        {
            RuleFor(x => x.Query).NotEmpty().MinimumLength(2).MaximumLength(200);
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 50);
        }
    }
}
