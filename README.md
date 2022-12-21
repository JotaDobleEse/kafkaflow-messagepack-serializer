# KafkaFlow MessagePack Serializer
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/2a86b45f0ec2487fb63dfd581071465a)](https://www.codacy.com/gh/Farfetch/kafkaflow-retry-extensions/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Farfetch/kafkaflow-retry-extensions&amp;utm_campaign=Badge_Grade)

KafkaFlow MessagePack Serializer is an extension of [Kafka Flow](https://github.com/Farfetch/kafkaflow) that use [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) library to optimize message sizes.

## Requirements
**.NET Core 2.1 and later**

## NuGet Package

|Name                             |nuget.org|
|---------------------------------|----|
|KafkaFlow.Serializer.MessagePack|[![Nuget Package](https://img.shields.io/nuget/v/KafkaFlow.Serializer.MessagePack.svg?logo=nuget)](https://www.nuget.org/packages/KafkaFlow.Serializer.MessagePack/) ![Nuget downloads](https://img.shields.io/nuget/dt/KafkaFlow.Serializer.MessagePack.svg)

## Install via NuGet 
    Install-Package KafkaFlow.Serializer.MessagePack

## Usage 
### For a specific message type

```csharp
.AddMiddlewares(
    middlewares => middlewares // KafkaFlow middlewares
    .AddSingleTypeSerializer<SampleMessage, MessagePackSerializer>()
    )
```

### For all message types

```csharp
.AddMiddlewares(
    middlewares => middlewares // KafkaFlow middlewares
    .AddSerializer<MessagePackSerializer>()
    )
```

See [samples](https://github.com/JotaDobleEse/kafkaflow-messagepack-serializer/tree/main/samples/) for more details

## Contributing

Read the [Contributing guidelines](CONTRIBUTING.md)

## Maintainers

-   [Jesús Ruiz](https://github.com/JotaDobleEse)


## License

[MIT](LICENSE.md)