using FluentValidation;
using Metflix.Models.Requests.Purchase;

namespace Metflix.Host.Validators.PurchaseValidators
{
    public class PurchaseRequestValidator : AbstractValidator<PurchaseRequest>
    {
        public PurchaseRequestValidator()
        {
            RuleFor(x=>x.MovieIds)
                .Must(x=>x.All(y=>y>0)).WithMessage(ValidationMessages.MovieIdPositiveNumber);

            RuleFor(x => x.Days)
                .GreaterThan(0);
        }
    }
}
