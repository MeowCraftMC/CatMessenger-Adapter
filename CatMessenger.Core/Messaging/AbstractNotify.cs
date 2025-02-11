using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CatMessenger.Core.Messaging;

public abstract class AbstractNotify<TMessage>(
    string clientId,
    Func<IConnection> connection,
    ILogger logger) :
    AbstractQueue(clientId, connection, logger)
{
    public delegate void Handler(TMessage message);

    private readonly ILogger _logger = logger;

    public event Handler? OnMessage;

    protected override string GetExchangeType()
    {
        return "fanout";
    }

    protected override string GetRoutingKey()
    {
        return string.Empty;
    }

    protected override IAsyncBasicConsumer CreateConsumer()
    {
        return new NotifyConsumer<TMessage>(Channel!, this, _logger);
    }

    public async Task PublishAsync(TMessage message)
    {
        var json = JsonSerializer.Serialize(message, Constants.JsonSerializerOptions);
        var bytes = Encoding.UTF8.GetBytes(json);
        await PublishAsync(bytes);
    }

    private class NotifyConsumer<TMsg>(IChannel channel, AbstractNotify<TMsg> queue, ILogger logger)
        : AsyncDefaultBasicConsumer(channel)
    {
        public override async Task HandleBasicDeliverAsync(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IReadOnlyBasicProperties properties, ReadOnlyMemory<byte> body,
            CancellationToken cancellationToken = new())
        {
            if (queue.ClientId == properties.AppId)
            {
                await queue.AckAsync(deliveryTag);
                return;
            }

            var str = Encoding.UTF8.GetString(body.ToArray());
            var message = JsonSerializer.Deserialize<TMsg>(str, Constants.JsonSerializerOptions);

            if (message == null)
            {
                await queue.AckAsync(deliveryTag);
                return;
            }

            try
            {
                queue.OnMessage?.Invoke(message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error handling message: \n{}", message);
            }

            await queue.AckAsync(deliveryTag);
        }
    }
}