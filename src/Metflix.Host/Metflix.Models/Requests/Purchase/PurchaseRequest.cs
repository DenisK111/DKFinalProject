namespace Metflix.Models.Requests.Purchase
{
    public record PurchaseRequest
    {
        public int[] MovieIds { get; set; } = null!;
        public int Days { get; set; }
    }
}
