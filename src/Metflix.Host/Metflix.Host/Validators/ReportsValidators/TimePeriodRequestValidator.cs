using System.Globalization;
using FluentValidation;
using Metflix.Models.Requests.Reports;
using Utils;

namespace Metflix.Host.Validators.ReportsValidators
{
    public class TimePeriodRequestValidator : AbstractValidator<TimePeriodRequest>
    {
        public TimePeriodRequestValidator()
        {            
            RuleFor(c => c.StartDate)
            .MustBeValidDateTime<TimePeriodRequest, string>()
            .WithMessage(ValidationMessages.InvalidDateTimeFormat);    
            
            RuleFor(c => c.EndDate)!
            .MustBeValidDateTime<TimePeriodRequest, string>()
            .When(x => !string.IsNullOrWhiteSpace(x.EndDate))
            .WithMessage(ValidationMessages.InvalidDateTimeFormat);      
            
            RuleFor(x=>x).Must(x=> x.StartDate.TryParseExactAcceptableFormats() < x.EndDate!.TryParseExactAcceptableFormats())
                .When(x=> !string.IsNullOrWhiteSpace(x.EndDate) 
                && DateTime.TryParse(x.StartDate,CultureInfo.InvariantCulture,DateTimeStyles.None,out _)
                && DateTime.TryParse(x.EndDate,CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime __))
                .WithMessage(ValidationMessages.StartDateMustBeBeforeEndDate);
        }
    }
}
