namespace Metflix.Models.Requests.Identity
{
    public class LoginRequest
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
