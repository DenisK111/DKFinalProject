using System.Globalization;
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

        public static IRuleBuilderOptions<T, string> MustBeValidDateTime<T, TElement>(this IRuleBuilder<T, string> ruleBuilder)
        {
            string[] formats = { "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd" };
            return ruleBuilder
             .Must(x => DateTime.TryParseExact(x, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))            
            .WithMessage("Date should be in yyyy-MM-dd format or yyyy-MM-dd hh:mm:ss format");
        }
    }
}

