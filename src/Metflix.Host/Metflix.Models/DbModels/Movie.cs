namespace Metflix.Models.DbModels
{
    public class Movie : BaseModel<int>
    {
        public string Name { get; init; } = null!;
        public int TotalQuantity { get; init; }
        public int AvailableQuantity { get; init; }

        public decimal PricePerDay { get; init; }

        public int Year { get; init; }
    }
}
