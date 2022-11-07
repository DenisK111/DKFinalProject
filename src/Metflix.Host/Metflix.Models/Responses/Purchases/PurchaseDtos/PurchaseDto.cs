using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Metflix.Models.DbModels;

namespace Metflix.Models.Responses.Purchases.PurchaseDtos
{
    public record PurchaseDto
    {
        public string Id { get; set; } = null!;
        public IEnumerable<MovieRecord> Movies { get; set; } = null!;

        public int Days { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime DueDate { get; set; }

        public Guid UserId { get; set; }

        public IEnumerable<int> UserMovieIds { get; set; } = null!;
    }
}
