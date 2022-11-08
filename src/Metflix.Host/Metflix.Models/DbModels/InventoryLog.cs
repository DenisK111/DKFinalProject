using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.DbModels
{
    public class InventoryLog : BaseModel<Guid>
    {
        public string UserId { get; set; } = null!;
        
        public string UserName { get; set; } = null!;
        
        public int MovieId { get; set; }
        
        public string MovieName { get; set; } = null!;
        
        public int AmountChanged { get; set; }       
    }
}
