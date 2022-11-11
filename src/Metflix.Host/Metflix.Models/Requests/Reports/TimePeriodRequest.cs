namespace Metflix.Models.Requests.Reports
{
    public class TimePeriodRequest
    {
        public string StartDate { get; set; } = null!;
        public string? EndDate { get; set; }
    }
}
