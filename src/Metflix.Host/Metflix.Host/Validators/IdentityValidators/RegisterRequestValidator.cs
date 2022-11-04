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
            .Must(x => DateTime.TryParseExact(x, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            .When(x => !string.IsNullOrWhiteSpace(x.DateOfBirth))
            .WithMessage("Date should be in yyyy-MM-dd format");

            RuleFor(c => c.Email)
                .NotEmpty()
                .MaximumLength(200)
                .EmailAddress()
                .WithMessage(ValidationMessages.InvalidEmail);

            RuleFor(x => x.Password)
                     .NotEmpty()
                     .MinimumLength(6)
                     .Must((model, password) => password.Any(x => char.IsDigit(x)))
                     .Must((model, password) => password.Any(x => char.IsUpper(x)));

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);
          

        }
    }
}
