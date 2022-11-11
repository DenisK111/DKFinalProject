namespace Metflix.Models.DbModels
{
    public class BaseModel<T> 
    {
        public T Id { get; set; }
        public DateTime LastChanged { get; init; }
    }
}
