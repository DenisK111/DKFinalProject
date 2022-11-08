using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Metflix.Kafka.Contracts;
using Metflix.Kafka.Serialization;
using Metflix.Kafka.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Metflix.Kafka.Consumers
{
    public abstract class GenericConsumer<TKey, TValue, TSettings> : IHostedService,IDisposable
        where TSettings : KafkaConsumerSettings
        where TValue : class, IKafkaItem<TKey>
        where TKey : notnull
    {
        private readonly IConsumer<TKey, TValue> _consumer;
        public GenericConsumer(IOptionsMonitor<TSettings> kafkaSettings)
        {
            _consumer = new ConsumerBuilder<TKey, TValue>(new ConsumerConfig()
            {
                BootstrapServers = kafkaSettings.CurrentValue.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                GroupId = kafkaSettings.CurrentValue.GroupId
            })
                .SetKeyDeserializer(new MsgDeserializer<TKey>())
                .SetValueDeserializer(new MsgDeserializer<TValue>())
                .Build();
            _consumer.Subscribe(kafkaSettings.CurrentValue.Topic);            
        }

        public abstract Task HandleMessage(Message<TKey,TValue> message, CancellationToken cancellationToken);        

        public void Dispose()
        {
            _consumer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async() =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var result = _consumer.Consume();
                    var message = result.Message;
                    await HandleMessage(message, cancellationToken);
                }
            });

            return Task.CompletedTask;
            
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
