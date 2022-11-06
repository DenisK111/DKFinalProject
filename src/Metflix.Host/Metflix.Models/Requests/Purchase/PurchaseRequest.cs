using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Requests.Purchase
{
    public record PurchaseRequest
    {
        public int[] MovieIds { get; set; } = null!;
        public int Days { get; set; }
    }
}
