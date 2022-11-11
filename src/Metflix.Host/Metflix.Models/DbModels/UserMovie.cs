namespace Metflix.Models.DbModels
{
    public class UserMovie :BaseModel<int>
    {        
        public string UserId { get; init; } = null!;
        public int MovieId { get; init; }       
        public int DaysFor { get;init; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedOn { get; init; }

        public bool IsReturned { get; set; }
    }
}
