namespace KafkaFlow.Serializer.MessagePack.UnitTests
{
    using AutoFixture;
    using global::MessagePack;
    using global::MessagePack.Resolvers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Shouldly;
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
            output.Length.ShouldBeGreaterThan(0);
            output.Position.ShouldBeGreaterThan(0);
        }
    }
}
