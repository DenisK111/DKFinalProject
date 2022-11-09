namespace Utils
{
    public static class ResponseMessages
    {
        public static string IdNotFound { get;} = "Id not found.";
        public static string InvalidData { get;} = "Invalid Data.";
        public static string EmailExists { get;} = "Email already exists.";
        public static string InvalidCredentials { get;} = "InvalidCredentials";
        public static string SqlExceptionMessage { get;} = "Something went wrong. Please try again.";
        public static string SuccesfullyRegistered { get;} = "Registration Successful";
        public static string InternalServerErrorMessage { get;} = "Something went wrong on our end! We have alerted our Engineers.";
        public static string InvalidMovieIds { get;} = "The following Id's do not exist: {0}.";
        public static string NotEnoughQtyMovies { get;} = "The following Id's are currently not in stock: {0}.";
        public static string NoIdForUser { get;} = "You do not have a purchase with this Id.";
        public static string PendingPurchase { get;} = "You currently have a pending purchase. Please wait before your purchase completes before making a new purchase.";
        public static string MovieAlreadyReturned { get;} = "Movie has already been returned.";
        public static string NotEnoughInventoryToRemove { get;} = "Not enough inventory to remove";
        public static string InvalidUser { get;} = "Invalid User";
    }
}