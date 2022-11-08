using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using MessagePack;
using Metflix.Kafka.Contracts;

namespace Metflix.Models.KafkaModels
{
    [MessagePackObject]
    public class InventoryChangeData : IKafkaItem<Guid>
    {
        [Key(0)]
        public Guid Id { get;} = Guid.NewGuid();
        [Key(1)]
        public string UserId { get; set; } = null!;
        [Key(2)]
        public string UserName { get; set; } = null!;
        [Key(3)]
        public int MovieId { get; set; }
        [Key(4)]
        public string MovieName { get; set; } = null!;
        [Key(5)]
        public int AmountChanged { get; set; }
        [Key(6)]
        public DateTime LastChanged { get; set; }

        public Guid GetKey()
        {
            return Id;
        }
        
    }
}
