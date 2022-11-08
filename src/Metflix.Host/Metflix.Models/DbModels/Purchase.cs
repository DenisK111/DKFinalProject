using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;

namespace Metflix.Models.DbModels
{
    [MessagePackObject]
    public class Purchase 
    {
        [Key(0)]
        public Guid Id { get; set; }
        [Key(1)]
        public IEnumerable<MovieRecord> Movies { get; set; } = null!;
        [Key(2)]
        public int Days { get; set; }
        [Key(3)]
        public decimal TotalPrice { get; set; }
        [Key(4)]
        public DateTime PurchaseDate { get; set; }
        [Key(5)]
        public DateTime DueDate { get; set; }
        [Key(6)]
        public string UserId { get; set; } = null!;
        [Key(7)]
        public IEnumerable<int> UserMovieIds = null!;
        [Key(8)]
        public DateTime LastChanged { get; set; }
    }
}
