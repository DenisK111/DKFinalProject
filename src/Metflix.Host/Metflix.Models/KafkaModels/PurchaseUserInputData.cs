using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using Metflix.Kafka.Contracts;

namespace Metflix.Models.KafkaModels
{
    [MessagePackObject]
    public class PurchaseUserInputData : IKafkaItem<string>
    {
        [Key(0)]
        public string Id { get; set; } = null!;
        [Key(1)]
        public int[] MovieIds { get; set; } = null!;
        [Key(2)]
        public int Days { get; set; }

        public string GetKey()
        {
            return Id;
        }
    }
}
