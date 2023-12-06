namespace KafkaFlow.Serializer.MessagePack.UnitTests
{
    using AutoFixture;
    using FluentAssertions;
    using global::MessagePack;
    using global::MessagePack.Resolvers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    public class MessagePackDeserializerTests
    {
        private static readonly MessagePackSerializerOptions MessagePackOptions = MessagePackSerializerOptions
            .Standard
            .WithResolver(CompositeResolver.Create(
                NativeDateTimeResolver.Instance,
                StandardResolverAllowPrivate.Instance))
            .WithCompression(MessagePackCompression.Lz4BlockArray);

        private Mock<ISerializerContext> contextMock = new();

        private MessagePackDeserializer deserializer = new(MessagePackOptions);

        private Fixture fixture = new();

        [TestMethod]
        public async Task DeserializeAsync_ValidPayload_ObjectGenerated()
        {
            // Arrange
            var message = fixture.Create<TestMessage>();
            using var input = new MemoryStream();
            await MessagePackSerializer.SerializeAsync(message.GetType(), input, message, MessagePackOptions);
            input.Seek(0, SeekOrigin.Begin);

            // Act
            var result = await deserializer.DeserializeAsync(input, typeof(TestMessage), contextMock.Object);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<TestMessage>();
        }
    }
}
