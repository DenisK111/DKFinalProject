using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metflix.Kafka.Contracts
{
    public interface IKafkaItem<TKey>
    {
        public TKey GetKey();
    }
}
