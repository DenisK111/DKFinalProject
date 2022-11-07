using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.DbModels
{
    public class Purchase : BaseModel<Guid>
    {
        public IEnumerable<MovieRecord> Movies { get; set; } = null!;

        public int Days { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime DueDate { get; set; }

        public string UserId { get; set; } = null!;

        public IEnumerable<int> UserMovieIds = null!;
    }
}
