using FluentValidation;
using Metflix.Models.Requests.Movies;

namespace Metflix.Host.Validators.MovieValidators
{
    public class ByIntIdRequestValidator : AbstractValidator<ByIntIdRequest>
    {
        public ByIntIdRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
