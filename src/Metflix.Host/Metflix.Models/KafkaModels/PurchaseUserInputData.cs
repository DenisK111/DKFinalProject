﻿using MessagePack;
using Metflix.Kafka.Contracts;

namespace Metflix.Models.KafkaModels
{
    [MessagePackObject]
    public class PurchaseUserInputData : IKafkaItem<string>
    {
        [Key(0)]
        public string UserId { get; set; } = null!;
        [Key(1)]
        public int[] MovieIds { get; set; } = null!;
        [Key(2)]
        public int Days { get; set; }

        public string GetKey()
        {
            return UserId;
        }
    }
}
