

using FluentValidation;

namespace Horizon.Application.Features.Search.GetSearchSuggestions
{
    public class GetSearchSuggestionsValidator : AbstractValidator<GetSearchSuggestionsQuery>
    {
        public GetSearchSuggestionsValidator()
        {
            RuleFor(x => x.Query).NotEmpty().MinimumLength(2).MaximumLength(100);
        }
    }
}
