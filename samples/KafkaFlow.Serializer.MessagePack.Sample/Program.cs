namespace KafkaFlow.Serializer.MessagePack.Sample
{
    using global::MessagePack;
    using global::Microsoft.Extensions.DependencyInjection;
    using KafkaFlow;
    using Newtonsoft.Json;
    using System;
    using System.Threading.Tasks;
    using MessagePackSerializer = KafkaFlow.Serializer.MessagePackSerializer;

    internal static class Program
    {
        private static async Task Main()
        {
            var services = new ServiceCollection();

            const string producerName = "producer-test";
            const string consumerName = "consumer-test";

            services.AddKafka(
                kafka => kafka
                    .UseConsoleLog()
                    .AddCluster(
                        cluster => cluster
                            .WithBrokers(new[] { "localhost:9092" })
                            .AddProducer(
                                producerName,
                                producer => producer
                                    .DefaultTopic("flow-control-sample")
                                    .AddMiddlewares(m => m.AddSingleTypeSerializer<SampleMessage, MessagePackSerializer>())
                            )
                            .AddConsumer(
                                consumer => consumer
                                    .Topic("flow-control-sample")
                                    .WithGroupId("print-console-handler")
                                    .WithName(consumerName)
                                    .WithInitialState(ConsumerInitialState.Stopped)
                                    .WithBufferSize(100)
                                    .WithWorkersCount(1)
                                    .AddMiddlewares(
                                        m => m
                                            .AddSingleTypeSerializer<SampleMessage, MessagePackSerializer>()
                                            .Add<PrintConsoleMiddleware>()
                                    )
                            )
                    )
            );

            var provider = services.BuildServiceProvider();

            var bus = provider.CreateKafkaBus();

            await bus.StartAsync();

            var producer = bus.Producers[producerName];
            var consumer = bus.Consumers[consumerName];

            while (true)
            {
                Console.Write("Number of messages to produce, Start, Stop, Pause, Resume, or Exit:");
                var input = Console.ReadLine()?.ToLower();

                switch (input)
                {
                    case var _ when int.TryParse(input, out var count):
                        for (var i = 0; i < count; i++)
                        {
                            var message = new SampleMessage
                            {
                                Id = Guid.NewGuid(),
                                Name = $"Name_{Guid.NewGuid()}"
                            };

                            producer.Produce(message.Id.ToString(), message);
                        }

                        break;

                    case "pause":
                        consumer.Pause(consumer.Assignment);
                        Console.WriteLine("Consumer paused");

                        break;

                    case "resume":
                        consumer.Resume(consumer.Assignment);
                        Console.WriteLine("Consumer resumed");

                        break;

                    case "stop":
                        await consumer.StopAsync();
                        Console.WriteLine("Consumer stopped");

                        break;

                    case "start":
                        await consumer.StartAsync();
                        Console.WriteLine("Consumer started");

                        break;

                    case "exit":
                        await bus.StopAsync();
                        return;
                }
            }
        }
    }

    internal class PrintConsoleMiddleware : IMessageMiddleware
    {
        public Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            Console.WriteLine($"Message: {JsonConvert.SerializeObject(context.Message.Value)}");
            return Task.CompletedTask;
        }
    }

    [MessagePackObject]
    internal class SampleMessage
    {
        [Key(0)]
        public Guid Id { get; set; }

        [Key(1)]
        public string Name { get; set; }
    }
}
