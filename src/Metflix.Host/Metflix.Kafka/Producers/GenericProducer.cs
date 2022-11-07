using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Metflix.Kafka.Serialization;
using Metflix.Kafka.Settings;
using Microsoft.Extensions.Options;

namespace Metflix.Kafka.Producers
{
    public class GenericProducer<TKey, TValue, TSettings> : IDisposable
       where TSettings : KafkaProducerSettings
    {

        private readonly IProducer<TKey, TValue> _producer;
        private readonly IOptionsMonitor<TSettings> _producerSettings;
        private readonly TopicPartition _topicParition;

        public GenericProducer(IOptionsMonitor<TSettings> producerSettings)
        {
            _producerSettings = producerSettings;
            _producer = new ProducerBuilder<TKey, TValue>(new ProducerConfig
            {
                BootstrapServers = _producerSettings.CurrentValue.BootstrapServers,
            })
                .SetKeySerializer(new MsgSerializer<TKey>())
                .SetValueSerializer(new MsgSerializer<TValue>())
                .Build();

            _topicParition = new TopicPartition(_producerSettings.CurrentValue.Topic, new Partition(_producerSettings.CurrentValue.Partition));
        }

        public void Dispose()
        {
            _producer.Dispose();
        }

        public async Task ProduceAsync(TKey key, TValue value)
        {
            var msg = new Message<TKey, TValue>()
            {
                Key = key,
                Value = value,
            };

            await _producer.ProduceAsync(_topicParition, msg);
        }

    }
}
