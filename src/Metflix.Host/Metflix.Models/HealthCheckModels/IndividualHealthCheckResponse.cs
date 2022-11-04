using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Models.HealthCheckModels
{
    public class IndividualHealthCheckResponse
    {
        public string Status { get; set; } = null!;

        public string Component { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
