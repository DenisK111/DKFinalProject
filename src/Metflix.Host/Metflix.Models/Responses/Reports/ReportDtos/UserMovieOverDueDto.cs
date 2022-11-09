using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Responses.Reports.ReportDtos
{
    public class UserMovieOverDueDto
    {
        public int Id { get; set; }
        public string UserId { get; init; } = null!;
        public int MovieId { get; init; }
        public DateTime LastChanged { get; init; }
        public int DaysFor { get; init; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; init; }
        
    }
}
