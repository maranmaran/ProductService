using Common;
using FluentValidation;

namespace Business.Queries.GetProducts
{
    /// <summary>
    /// Validator for products query
    /// </summary>
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            // Price should not be negative
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).When(x => x.MaxPrice != null)
                .WithMessage("Maximum price can not be lesser than 0");

            // Size should not be empty string
            RuleFor(x => x.Size)
                .Transform(x => x?.Trim())
                .NotEmpty().When(x => x.Size != null)
                .WithMessage("Size must be not be empty string");

            // Any highlight keywords given should not be empty
            RuleForEach(x => x.HighlightKeywords)
                .Transform(x => x.Trim())
                .NotEmpty().When(x => !x.HighlightKeywords.IsNullOrEmpty())
                .WithMessage("Word to highlight must not be empty string");
        }
    }
}
