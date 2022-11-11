namespace Metflix.Models.Responses.Movies
{
    public class RemoveInventoryRequest
    {
        public int MovieId { get; set; }

        public int Quantity { get; set; }
    }
}
