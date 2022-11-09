using FluentValidation;
using Metflix.Models.Requests.Purchase;

namespace Metflix.Host.Validators.PurchaseValidators
{
    public class ReturnMovieRequestValidator : AbstractValidator<ReturnMovieRequest>
    {
        public ReturnMovieRequestValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty()
                .GreaterThan(0);
        }

    }
}
