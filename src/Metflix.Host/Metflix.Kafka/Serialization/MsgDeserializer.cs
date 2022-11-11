using Confluent.Kafka;
using MessagePack;

namespace Metflix.Kafka.Serialization
{
    public class MsgDeserializer<T> : IDeserializer<T>
    {
        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return MessagePackSerializer.Deserialize<T>(data.ToArray());
        }
    }
}
