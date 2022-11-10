using Metflix.Kafka.Settings;

namespace Metflix.Kafka.Contracts
{
    public interface IGenericProducer<TKey, TValue, TSettings> : IDisposable
        where TSettings : KafkaProducerSettings
        where TKey : notnull
        where TValue : class, IKafkaItem<TKey>
    {
        Task ProduceAsync(TKey key, TValue value, CancellationToken cancellationToken = default);
    }
}