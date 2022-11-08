using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Kafka.Settings
{
    public abstract record KafkaConsumerSettings
    {
        public string BootstrapServers { get; set; } = null!;
        public string Topic { get; set; } = null!;
        public string GroupId { get; set; } = null!;
    }
}
