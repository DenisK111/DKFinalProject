using Utils;

namespace Metflix.Host.Validators
{
    public static class ValidationMessages
    {
        public static string AvailableQtyValidation { get;} = "Available quantity cannot be more than total quantity.";
        public static string InvalidMovieName { get;} = "Movie must be between 1 and 200 characters in length";
        public static string InvalidEmail { get;} = "Invalid Email.";
        public static string MovieIdPositiveNumber { get;} = "Movie Id's must be a positive integer";
        public static string StartDateMustBeBeforeEndDate { get;} = "Start date must be before end date.";
        public static string PasswordsMustMatch { get;} = "Password and ConfirmPassword must match";
        public static string InvalidDateTimeFormat { get;} = $"Invalid Date. Date should be a valid Date and should be in one of the following formats: {Environment.NewLine}{string.Join(Environment.NewLine,DateTimeFormats.AcceptableInputFormats)}";
    }
}
