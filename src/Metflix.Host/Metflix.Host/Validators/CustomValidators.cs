using FluentValidation;
using Metflix.Models.Requests.Movies;

namespace Metflix.Host.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, string> MustBeValidMovieName<T, TElement>(this IRuleBuilder<T, string> ruleBuilder) 
        {
            return ruleBuilder.NotEmpty()
                .MaximumLength(200)
                .WithMessage(ValidationMessages.InvalidMovieName);
        }
    }
}

