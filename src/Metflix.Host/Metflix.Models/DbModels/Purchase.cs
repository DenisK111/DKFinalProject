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

        public int TotalPrice { get; set; }

        public DateTime PurchaseDate { get; set; }

        public Guid UserId { get; set; }
    }
}
