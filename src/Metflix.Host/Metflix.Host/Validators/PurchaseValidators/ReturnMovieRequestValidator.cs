using FluentValidation;
using Metflix.Models.Requests.Purchase;

namespace Metflix.Host.Validators.PurchaseValidators
{
    public class ReturnMovieRequestValidator : AbstractValidator<ReturnMovieRequest>
    {
        public ReturnMovieRequestValidator()
        {
            RuleFor(x => x.UserMovieId)
                .NotEmpty()
                .GreaterThan(0);
        }

    }
}
