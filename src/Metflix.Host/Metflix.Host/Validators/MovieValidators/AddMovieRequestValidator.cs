using FluentValidation;
using Metflix.Models.Requests.Movies;

namespace Metflix.Host.Validators.MovieValidators
{
    public class AddMovieRequestValidator : AbstractValidator<AddMovieRequest>
    {
        public AddMovieRequestValidator()
        {
            RuleFor(x => x.Name)
                .MustBeValidMovieName<AddMovieRequest,string>();

            RuleFor(x=>x.TotalQuantity)
                .NotEmpty()
                .GreaterThan(0)
                .LessThanOrEqualTo(30);

            RuleFor(x => x.PricePerDay)
                .NotEmpty()
                .GreaterThan(0)
                .LessThanOrEqualTo(20);

            RuleFor(x=>x.Year)
                .NotEmpty()
                .GreaterThanOrEqualTo(1900)
                .LessThanOrEqualTo(DateTime.Now.Year);
        }
    }
}
