using System.Globalization;
using FluentValidation;
using Metflix.Models.Requests.Identity;

namespace Metflix.Host.Validators.IdentityValidators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(c => c.DateOfBirth)
            .MustBeValidDateTime<RegisterRequest, string>()
            .WithMessage(ValidationMessages.InvalidDateTimeFormat);

            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(200)
                .EmailAddress()
                .WithMessage(ValidationMessages.InvalidEmail);


            RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(4)
            .Must((model, password) => password.Any(x => char.IsLetter(x) || char.IsDigit(x)));

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x)
                .Must(x => x.Password == x.ConfirmPassword)
                .WithMessage(ValidationMessages.PasswordsMustMatch);

        }
    }
}
