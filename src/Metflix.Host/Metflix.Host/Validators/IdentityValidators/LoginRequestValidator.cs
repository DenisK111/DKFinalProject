using System.Data;
using FluentValidation;
using Metflix.Models.Requests.Identity;

namespace Metflix.Host.Validators.IdentityValidators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                .DependentRules(() =>
                {
                    RuleFor(x => x.Password)
                     .NotEmpty()
                     .MinimumLength(4)
                     .Must((model, password) => password.Any(x => char.IsLetter(x) || char.IsDigit(x)));
                     
                });
                        
        }
    }
}
