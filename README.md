# KafkaFlow MessagePack Serializer
KafkaFlow MessagePack Serializer is an extension of [Kafka Flow](https://github.com/Farfetch/kafkaflow) that use [MessagePack-CSharp](https://github.com/neuecc/MessagePack-CSharp) library to optimize message sizes.

## Requirements

**.NET Core 2.1 and later**

## NuGet Package

|Name                             |nuget.org|
|---------------------------------|----|
|KafkaFlow.Serializer.MessagePack|[![Nuget Package](https://img.shields.io/nuget/v/KafkaFlow.Serializer.MessagePack.svg?logo=nuget)](https://www.nuget.org/packages/KafkaFlow.Serializer.MessagePack/) ![Nuget downloads](https://img.shields.io/nuget/dt/KafkaFlow.Serializer.MessagePack.svg)

## Install via NuGet

```bash
Install-Package KafkaFlow.Serializer.MessagePack
```

## Usage

### Consumer

#### Deserialize a specific message type

```csharp
.AddMiddlewares(
    middlewares => middlewares // KafkaFlow middlewares
    .AddSingleTypeDeserializer<SampleMessage, MessagePackDeserializer>()
    )
```

#### Deserialize all message types

```csharp
.AddMiddlewares(
    middlewares => middlewares // KafkaFlow middlewares
    .AddDeserializer<MessagePackDeserializer>()
    )
```

### Producer

#### Serialize a specific message type

```csharp
.AddMiddlewares(
    middlewares => middlewares // KafkaFlow middlewares
    .AddSingleTypeSerializer<SampleMessage, MessagePackSerializer>()
    )
```

#### Serialize all message types

```csharp
.AddMiddlewares(
    middlewares => middlewares // KafkaFlow middlewares
    .AddSerializer<MessagePackSerializer>()
    )
```

See [samples](https://github.com/JotaDobleEse/kafkaflow-messagepack-serializer/tree/main/samples/) for more details

## Maintainers

-   [Jesús Ruiz](https://github.com/JotaDobleEse)

## License

[MIT](LICENSE.md)
