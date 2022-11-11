namespace Metflix.Kafka.Settings
{
    public abstract record KafkaProducerSettings
    {
        public string BootstrapServers { get; set; } = null!;
        public string Topic { get; set; } = null!;        
    }
}
