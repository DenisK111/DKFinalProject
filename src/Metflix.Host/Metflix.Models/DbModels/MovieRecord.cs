namespace Metflix.Models.DbModels
{
    public class MovieRecord
    {
        public int MovieId { get; set; }

        public string MovieName { get; set; } = null!;

        public decimal PricePerDay { get; set; }
    }
}