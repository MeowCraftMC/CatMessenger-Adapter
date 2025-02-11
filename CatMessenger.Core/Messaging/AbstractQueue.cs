using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CatMessenger.Core.Messaging;

public abstract class AbstractQueue(string clientId, Func<IConnection> connection, ILogger logger)
{
    private static readonly int MaxRetry = 5;

    public bool Closed { get; private set; } = true;

    public IChannel? Channel { get; private set; }

    public string ClientId => clientId;

    protected abstract string GetExchangeName();
    protected abstract string GetExchangeType();
    protected abstract string GetQueueName();
    protected abstract string GetRoutingKey();
    protected abstract IAsyncBasicConsumer CreateConsumer();

    public async Task ConnectAsync()
    {
        Channel ??= await connection.Invoke().CreateChannelAsync();

        Closed = false;

        await Channel.ExchangeDeclareAsync(GetExchangeName(), GetExchangeType(), true, false);
        await Channel.QueueDeclareAsync(GetQueueName(), true, true, true);
        await Channel.QueueBindAsync(GetQueueName(), GetExchangeName(), GetRoutingKey());
        await Channel.BasicConsumeAsync(GetQueueName(), false, CreateConsumer());
    }

    public async Task DisconnectAsync()
    {
        if (!Closed)
        {
            Closed = true;

            if (Channel is not null && Channel.IsOpen) await Channel.CloseAsync();
        }
    }

    protected async Task PublishAsync(byte[] bytes)
    {
        var tried = 0;
        while (tried <= MaxRetry)
        {
            try
            {
                if (Closed) return;

                if (Channel is null) await ConnectAsync();

                var props = new BasicProperties
                {
                    AppId = clientId
                };
                await Channel!.BasicPublishAsync(GetExchangeName(), GetRoutingKey(), true, props, bytes);
                return;
            }
            catch (Exception ex)
            {
                tried += 1;
                logger.LogWarning(ex, "Publish failed, retrying({}/{})", tried, MaxRetry);
            }

            logger.LogError("All publish retries failed!");
        }
    }

    protected async Task AckAsync(ulong deliveryTag)
    {
        var tried = 0;
        while (tried <= MaxRetry)
        {
            try
            {
                if (Closed) return;

                if (Channel is null) await ConnectAsync();

                var props = new BasicProperties
                {
                    AppId = clientId
                };
                await Channel!.BasicAckAsync(deliveryTag, false);
                return;
            }
            catch (Exception ex)
            {
                tried += 1;
                logger.LogWarning(ex, "Ack failed, retrying({}/{})", tried, MaxRetry);
            }

            logger.LogError("All ack retries failed!");
        }
    }
}