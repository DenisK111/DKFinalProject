using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.Requests.Reports
{
    public class TimePeriodRequest
    {
        public string StartDate { get; set; } = null!;
        public string? EndDate { get; set; }
    }
}
