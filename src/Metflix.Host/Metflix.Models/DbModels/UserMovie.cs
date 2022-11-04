using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.DbModels
{
    public class UserMovie 
    {
        public int Id { get; set; }
        public Guid UserId { get; init; }
        public int MovieId { get; init; }
        public DateTime LastChanged { get; init; }
        public int DaysFor { get;init; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; init; }

        public bool IsReturned { get; set; }
    }
}
