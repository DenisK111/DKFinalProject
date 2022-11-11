using Metflix.Models.KafkaModels;

namespace Metflix.Models.DataflowModels
{
    public record FullPurchaseInfoData
    {      
        public string UserId { get; set; } = null!;        
        public IEnumerable<MovieInfoData> MovieInfoData { get; set; } = null!;        
        public int Days { get; set; }
        public IEnumerable<int> UserMovieIds { get; set; } = null!; 
    }
}
