namespace Utils
{
    public static class ResponseMessages
    {
        public static string IdNotFound { get; set; } = "Id not found.";
        public static string InvalidData { get; set; } = "Invalid Data.";
        public static string EmailExists { get; set; } = "Email already exists.";
        public static string InvalidCredentials { get; set; } = "InvalidCredentials";
        public static string SqlExceptionMessage { get; set; } = "Something went wrong. Please try again.";
        public static string SuccesfullyRegistered { get; set; } = "Registration Successful";
        public static string InternalServerErrorMessage { get; set; } = "Something went wrong on our end! We have alerted our Engineers.";
    }
}