namespace Metflix.Models.DbModels
{
    public class MovieRecord
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal PricePerDay { get; set; }
    }
}