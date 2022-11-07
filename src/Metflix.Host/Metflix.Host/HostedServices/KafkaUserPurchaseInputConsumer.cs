using Confluent.Kafka;
using Metflix.BL.Dataflow.Contracts;
using Metflix.Kafka.Consumers;
using Metflix.Models.Configurations.KafkaSettings.Consumers;
using Metflix.Models.KafkaModels;
using Microsoft.Extensions.Options;

namespace Metflix.Host.HostedServices
{
    public class KafkaUserPurchaseInputConsumer : GenericConsumer<string, PurchaseUserInputData, KafkaUserPurchaseInputConsumerSettings>
    {
        private readonly IPurchaseUserInputDataflow _purchaseUserInputDataFlow;
        public KafkaUserPurchaseInputConsumer(IOptionsMonitor<KafkaUserPurchaseInputConsumerSettings> kafkaSettings, IPurchaseUserInputDataflow purchaseUserInputDataFlow) : base(kafkaSettings)
        {
            _purchaseUserInputDataFlow = purchaseUserInputDataFlow;
        }

        public override async Task HandleMessage(Message<string, PurchaseUserInputData> message, CancellationToken cancellationToken)
        {
            await _purchaseUserInputDataFlow.ProcessData(message.Value,cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _purchaseUserInputDataFlow.Dispose();
            return base.StopAsync(cancellationToken);
        }

    }
}
