using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Responses.Movies
{
    public class AddInventoryRequest
    {
        public int MovieId { get; set; }

        public int Quantity { get; set; }
    }
}
