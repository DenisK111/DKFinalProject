using Confluent.Kafka;
using Metflix.BL.Dataflow.Contracts;
using Metflix.Kafka.Consumers;
using Metflix.Models.Configurations.KafkaSettings.Consumers;
using Metflix.Models.KafkaModels;
using Microsoft.Extensions.Options;

namespace Metflix.Host.HostedServices
{
    public class KafkaPurchaseDataConsumer : GenericConsumer<string, PurchaseInfoData, KafkaPurchaseDataConsumerSettings>
    {
        private readonly IPurchaseInfoDataflow _purchaseInfoDataflow;

        public KafkaPurchaseDataConsumer(IOptionsMonitor<KafkaPurchaseDataConsumerSettings> kafkaSettings, IPurchaseInfoDataflow purchaseInfoDataflow) : base(kafkaSettings)
        {
            _purchaseInfoDataflow = purchaseInfoDataflow;
        }

        public override async Task HandleMessage(Message<string, PurchaseInfoData> message, CancellationToken cancellationToken)
        {
            await _purchaseInfoDataflow.ProcessData(message.Value);
        }
    }
}
