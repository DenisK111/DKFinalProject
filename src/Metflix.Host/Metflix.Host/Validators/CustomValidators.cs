﻿using System.Globalization;
using FluentValidation;
using Metflix.Models.Requests.Movies;
using Utils;

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
            return ruleBuilder
             .Must(x => DateTime.TryParseExact(x.Trim(), DateTimeFormats.AcceptableInputFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out _));            
            
        }
    }
}

