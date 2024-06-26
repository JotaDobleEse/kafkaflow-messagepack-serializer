﻿namespace KafkaFlow.Serializer
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using MessagePack;
    using MessagePack.Resolvers;

    /// <summary>
    /// A message serializer using MessagePack library
    /// </summary>
    public class MessagePackDeserializer : IDeserializer
    {
        private static readonly IFormatterResolver DefaultResolver = CompositeResolver.Create(
                                                                        NativeDateTimeResolver.Instance,
                                                                        StandardResolverAllowPrivate.Instance);

        private readonly MessagePackSerializerOptions options;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePackDeserializer"/> class.
        /// </summary>
        /// <param name="options">MessagePack serializer options</param>
        public MessagePackDeserializer(MessagePackSerializerOptions options)
        {
            this.options = options;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessagePackDeserializer"/> class.
        /// </summary>
        public MessagePackDeserializer()
            : this(MessagePackSerializerOptions
                                .Standard
                                .WithResolver(DefaultResolver)
                                .WithCompression(MessagePackCompression.Lz4BlockArray))
        {
        }

        /// <inheritdoc/>
        public async Task<object> DeserializeAsync(Stream input, Type type, ISerializerContext context)
            => await MessagePack.MessagePackSerializer.DeserializeAsync(type, input, this.options);
    }
}
