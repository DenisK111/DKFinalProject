namespace Metflix.Models.Requests.Movies
{
    public class AddMovieRequest
    {
        public string Name { get; init; } = null!;
        public int TotalQuantity { get; init; }

        public decimal PricePerDay { get; init; }

        public int Year { get; init; }
    }
}
