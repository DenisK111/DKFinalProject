using MessagePack;

namespace Metflix.Models.DbModels
{
    [MessagePackObject]
    public class MovieRecord
    {
        [Key(0)]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; } = null!;
        [Key(2)]
        public decimal PricePerDay { get; set; }
    }
}