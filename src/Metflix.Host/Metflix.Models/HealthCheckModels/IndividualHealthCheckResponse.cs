namespace Metflix.Models.HealthCheckModels
{
    public class IndividualHealthCheckResponse
    {
        public string Status { get; set; } = null!;

        public string Component { get; set; } = null!;

        public string Description { get; set; } = null!;
    }
}
