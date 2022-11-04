namespace Metflix.Host.Validators
{
    public static class ValidationMessages
    {
        public static string AvailableQtyValidation { get; set; } = "Available quantity cannot be more than total quantity.";
        public static string InvalidMovieName { get; set; } = "Movie must be between 1 and 200 characters in length";
        public static string InvalidEmail { get; set; } = "Invalid Email.";
    }
}
