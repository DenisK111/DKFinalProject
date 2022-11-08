using AutoMapper;
using Confluent.Kafka;
using Metflix.DL.Repositories.Contracts;
using Metflix.Kafka.Consumers;
using Metflix.Models.Configurations.KafkaSettings.Consumers;
using Metflix.Models.DbModels;
using Metflix.Models.KafkaModels;
using Microsoft.Extensions.Options;

namespace Metflix.Host.HostedServices
{
    public class KafkaInventoryChangesConsumer : GenericConsumer<Guid, InventoryChangeData, KafkaInventoryChangesConsumerSettings>
    {
        private readonly IInventoryLogRepository _inventoryLogRepository;
        private readonly IMapper _mapper;

        public KafkaInventoryChangesConsumer(IOptionsMonitor<KafkaInventoryChangesConsumerSettings> kafkaSettings, IInventoryLogRepository inventoryLogRepository, IMapper mapper) : base(kafkaSettings)
        {
            _inventoryLogRepository = inventoryLogRepository;
            _mapper = mapper;
        }

        public override async Task HandleMessage(Message<Guid, InventoryChangeData> message, CancellationToken cancellationToken)
        {
            var inventoryLog = _mapper.Map<InventoryLog>(message.Value);
            await _inventoryLogRepository.AddLog(inventoryLog, cancellationToken);
        }
    }
}
