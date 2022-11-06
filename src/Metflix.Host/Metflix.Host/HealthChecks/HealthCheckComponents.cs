namespace Metflix.Host.HealthChecks
{
    public static class HealthCheckComponents
    {
        public static string SqlServer { get; set; } = "Sql Server";
        public static string MongoDb { get; set; } = "MongoDb";
    }
}
