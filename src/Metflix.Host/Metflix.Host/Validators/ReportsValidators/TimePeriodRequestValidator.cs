using System.Globalization;
using FluentValidation;
using Metflix.Models.Requests;

namespace Metflix.Host.Validators.ReportsValidators
{
    public class TimePeriodRequestValidator : AbstractValidator<TimePeriodRequest>
    {
        public TimePeriodRequestValidator()
        {
            string[] formats = { "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd" };
            RuleFor(c => c.StartDate)
            .MustBeValidDateTime<TimePeriodRequest, string>();    
            
            RuleFor(c => c.EndDate)!
            .MustBeValidDateTime<TimePeriodRequest, string>()
            .When(x => !string.IsNullOrWhiteSpace(x.EndDate));
        }
    }
}
