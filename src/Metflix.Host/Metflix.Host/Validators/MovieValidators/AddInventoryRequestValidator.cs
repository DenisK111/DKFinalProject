using FluentValidation;
using Metflix.Models.Responses.Movies;

namespace Metflix.Host.Validators.MovieValidators
{
    public class AddInventoryRequestValidator : AbstractValidator<AddInventoryRequest>
    {
        public AddInventoryRequestValidator()
        {
            RuleFor(x=>x.MovieId).NotEmpty().GreaterThan(0);
            RuleFor(x=>x.Quantity).NotEmpty().GreaterThan(0);
        }
    }
}
