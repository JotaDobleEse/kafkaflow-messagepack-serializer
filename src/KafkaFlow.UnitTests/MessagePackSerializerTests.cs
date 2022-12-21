namespace KafkaFlow.Serializer.MessagePack.UnitTests
{
    using AutoFixture;
    using FluentAssertions;
    using global::MessagePack;
    using global::MessagePack.Resolvers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    public class MessagePackSerializerTests
    {
        private static readonly MessagePackSerializerOptions MessagePackOptions = MessagePackSerializerOptions
                                                                                                .Standard
                                                                                                .WithResolver(CompositeResolver.Create(
                                                                                                              NativeDateTimeResolver.Instance,
                                                                                                              StandardResolverAllowPrivate.Instance))
                                                                                                .WithCompression(MessagePackCompression.Lz4BlockArray);

        private Mock<ISerializerContext> contextMock = new();

        private KafkaFlow.Serializer.MessagePackSerializer serializer = new(MessagePackOptions);

        private Fixture fixture = new();

        [TestMethod]
        public async Task SerializeAsync_ValidPayload_JsonByteArrayGenerated()
        {
            // Arrange
            var message = fixture.Create<TestMessage>();
            using var output = new MemoryStream();

            // Act
            await serializer.SerializeAsync(message, output, contextMock.Object);

            // Assert
            output.Length.Should().BeGreaterThan(0);
            output.Position.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task DeserializeAsync_ValidPayload_ObjectGenerated()
        {
            // Arrange
            var message = fixture.Create<TestMessage>();
            using var input = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(message.GetType(), input, message, MessagePackOptions);
            input.Seek(0, SeekOrigin.Begin);

            // Act
            var result = await serializer.DeserializeAsync(input, typeof(TestMessage), contextMock.Object);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TestMessage>();
        }

        [MessagePackObject]
        private class TestMessage
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
}
