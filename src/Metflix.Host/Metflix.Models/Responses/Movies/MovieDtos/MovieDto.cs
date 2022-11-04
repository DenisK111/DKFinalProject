using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Responses.Movies.MovieDtos
{
    public record MovieDto 
    {        
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public int TotalQuantity { get; init; }
        public int AvailableQuantity { get; init; }

        public decimal PricePerDay { get; init; }

        public int Year { get; init; }
        public DateTime LastChanged { get; init; }
        
    }
}
