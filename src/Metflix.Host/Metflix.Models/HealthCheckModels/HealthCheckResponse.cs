using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.HealthCheckModels
{
    public class HealthCheckResponse
    {
        public string Status { get; set; } = null!;
        public IEnumerable<IndividualHealthCheckResponse> HealthChecks { get; set; } = null!;

        public TimeSpan HealthCheckDuration { get; set; }
    }
}
