using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Requests.Movies
{
    public class UpdateMovieRequest
    {
        public int Id { get; set; }
        public string Name { get; init; } = null!;
        public int TotalQuantity { get; init; }

        public int AvailableQuantity { get; set; }

        public decimal PricePerDay { get; init; }

        public int Year { get; init; }
    }
}
