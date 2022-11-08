using FluentValidation;
using Metflix.Models.Responses.Movies;

namespace Metflix.Host.Validators.MovieValidators
{
    public class RemoveInventoryValidator : AbstractValidator<RemoveInventoryRequest>
    {
        public RemoveInventoryValidator()
        {
            RuleFor(x => x.MovieId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Quantity).NotEmpty().GreaterThan(0);
        }
    
    }
}
