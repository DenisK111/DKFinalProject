namespace Metflix.Kafka.Contracts
{
    public interface IKafkaItem<TKey>
    {
        public TKey GetKey();
    }
}
