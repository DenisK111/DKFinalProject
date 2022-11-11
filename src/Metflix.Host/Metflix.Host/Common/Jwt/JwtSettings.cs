namespace Metflix.Host.Common.Jwt
{
    public class JwtSettings
    {
        public const string Scheme = "bearer";
        public static string BearerFormat => "JWT";
        public static string Name => "JWT Authentication";
        public static string Description => "Put **_ONLY_** your JWT Bearer token in the textbox below";
    }
}
