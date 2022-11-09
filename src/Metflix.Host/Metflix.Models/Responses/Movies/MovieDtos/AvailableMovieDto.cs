using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Responses.Movies.MovieDtos
{
    public record AvailableMovieDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = null!;
        public decimal PricePerDay { get; init; }
        public int Year { get; init; }
    }
}
