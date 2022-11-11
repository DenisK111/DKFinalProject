using Confluent.Kafka;
using MessagePack;

namespace Metflix.Kafka.Serialization
{
    public class MsgSerializer<T> : ISerializer<T>
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return MessagePackSerializer.Serialize(data);
        }
    }
}
