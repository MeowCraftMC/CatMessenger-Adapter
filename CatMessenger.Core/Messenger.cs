using CatMessenger.Core.Config;
using CatMessenger.Core.Messaging;
using CatMessenger.Core.Model;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CatMessenger.Core;

public class Messenger
{
    private IConfigProvider _config;

    public Messenger(IConfigProvider config, ILogger<Messenger> logger)
    {
        _config = config;

        Factory = new ConnectionFactory
        {
            HostName = config.GetRabbitMqHost(),
            Port = config.GetRabbitMqPort(),
            VirtualHost = config.GetRabbitMqVirtualHost(),
            UserName = config.GetRabbitMqUsername(),
            Password = config.GetRabbitMqPassword(),
            AutomaticRecoveryEnabled = true
        };

        Message = new MessageNotify(config.GetId(), GetConnection, logger);
    }

    private ConnectionFactory Factory { get; }
    private IConnection? Connection { get; set; }

    public AbstractNotify<Message> Message { get; }

    public async Task ConnectAsync()
    {
        Connection = await Factory.CreateConnectionAsync();
        await Message.ConnectAsync();
    }

    public async Task DisconnectAsync()
    {
        await Message.DisconnectAsync();
    }

    private IConnection GetConnection()
    {
        return Connection!;
    }

    private class MessageNotify(string clientId, Func<IConnection> connection, ILogger logger) :
        AbstractNotify<Message>(clientId, connection, logger)
    {
        protected override string GetExchangeName()
        {
            return "fanout.exchange.messages";
        }

        protected override string GetQueueName()
        {
            return "fanout.queue." + ClientId;
        }
    }
}