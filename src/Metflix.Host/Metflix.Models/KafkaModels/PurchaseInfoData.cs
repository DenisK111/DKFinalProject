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
    public record PurchaseInfoData : IKafkaItem<string>
    {
        [Key(0)]
        public string UserId { get; set; } = null!;
        [Key(1)]
        public IEnumerable<MovieInfoData> MovieInfoData { get; set; } = null!;
        [Key(2)]
        public int Days { get; set; }
      


        public string GetKey()
        {
            return UserId;
        }
    }
}
