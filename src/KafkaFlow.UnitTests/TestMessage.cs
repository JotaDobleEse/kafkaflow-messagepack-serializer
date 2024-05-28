using System;
using MessagePack;

namespace KafkaFlow.Serializer.MessagePack.UnitTests
{
    [MessagePackObject]
    internal class TestMessage
    {
        [Key(0)]
        public int IntegerField { get; set; }

        [Key(1)]
        public string StringField { get; set; }

        [Key(2)]
        public double DoubleField { get; set; }

        [Key(3)]
        public DateTime DateTimeField { get; set; }
    }
}
