using System.Runtime.CompilerServices;
using FluentValidation;
using Metflix.Models.Requests.Movies;

namespace Metflix.Host.Validators.MovieValidators
{
    public class UpdateMovieRequestValidator : AbstractValidator<UpdateMovieRequest>
    {
        public UpdateMovieRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .GreaterThan(0);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.TotalQuantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .LessThanOrEqualTo(30);

            RuleFor(x => x.PricePerDay)
                .NotEmpty()
                .GreaterThan(0)
                .LessThanOrEqualTo(20);

            RuleFor(x => x.Year)
                .NotEmpty()
                .GreaterThanOrEqualTo(1900)
                .LessThanOrEqualTo(DateTime.Now.Year);

            RuleFor(x => x.AvailableQuantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x)
                .Must(x => x.AvailableQuantity <= x.TotalQuantity)
                .WithMessage(ValidationMessages.AvailableQtyValidation);



        }
    }
}
