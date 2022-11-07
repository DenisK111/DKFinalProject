using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Responses.Purchases.PurchaseDtos
{
    public class UserMovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime DueDate { get; set; }
    }
}
