using System.Text;
using CatMessenger.Core.Config;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace CatMessenger.Core.Connector;

public class MessageQueue(IConfigProvider config, ILogger<RabbitMqConnector> logger, IConnection connection)
    : IDisposable
{
    public static string ExchangeName { get; } = "messages";
    public static string ExchangeType { get; } = "fanout";

    public delegate void Handle(ConnectorMessage message);

    public event Handle? OnMessage;

    private IChannel? Channel { get; set; }

    private string QueueName { get; set; } = $"message.{config.GetId()}";

    public bool IsClosing { get; private set; } = false;

    public async Task Connect()
    {
        Channel = await connection.CreateChannelAsync();

        await Channel.ExchangeDeclareAsync(ExchangeName, ExchangeType, true, false);
        await Channel.QueueDeclareAsync(queue: QueueName, durable: true, exclusive: false, autoDelete: true);
        await Channel.QueueBindAsync(QueueName, ExchangeName, string.Empty);

        await Channel.BasicConsumeAsync(QueueName, false, new MessageConsumer(config, logger, this));
    }

    public async Task Disconnect()
    {
        await Channel.CloseAsync();
    }

    public void Dispose()
    {
        if (Channel is not null && Channel.IsOpen)
        {
            Channel.Dispose();
        }
    }

    public async Task Publish(ConnectorMessage message)
    {
        message.Client = config.GetId();
        message.Time ??= DateTime.Now;

        var json = JsonConvert.SerializeObject(message, Constants.JsonSerializerSettings);
        await InternalPublish(json, 0);
    }

    private async Task InternalPublish(string json, int retry)
    {
        try
        {
            if (IsClosing)
            {
                return;
            }

            if (retry > config.GetConnectorMaxRetry())
            {
                logger.LogError("Publish failed: exceed max retry");
            }

            if (Channel is null || !Channel.IsOpen)
            {
                Disconnect().Wait();
                Connect().Wait();
            }

            await Channel!.BasicPublishAsync(ExchangeName, string.Empty, Encoding.UTF8.GetBytes(json));
        }
        catch (Exception ex)
        {
            await InternalPublish(json, retry + 1);
        }
    }

    private class MessageConsumer(IConfigProvider config, ILogger<RabbitMqConnector> logger, MessageQueue queue)
        : DefaultBasicConsumer
    {
        public override async Task HandleBasicDeliverAsync(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, ReadOnlyBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            if (queue.IsClosing)
            {
                return;
            }

            var json = Encoding.UTF8.GetString(body.ToArray());

            var message = JsonConvert.DeserializeObject<ConnectorMessage>(json);

            if (message == null || message.Client == config.GetId())
            {
                await Channel.BasicAckAsync(deliveryTag, false);
                return;
            }

            try
            {
                queue.OnMessage?.Invoke(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process: {Json}", json);
            }

            await Channel.BasicAckAsync(deliveryTag, false);
        }
    }
}