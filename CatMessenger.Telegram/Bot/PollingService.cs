using CatMessenger.Core;
using CatMessenger.Telegram.Bot.Bases;
using CatMessenger.Telegram.Config;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace CatMessenger.Telegram.Bot;

public class PollingService(
    IServiceProvider serviceProvider,
    ILogger<PollingService> logger,
    ITelegramBotClient bot,
    ConfigProvider config,
    Messenger messenger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger)
{
    private int _tries;

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        while (_tries < 5)
            try
            {
                await InnerStartAsync(cancellationToken);
                break;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error on PollingService StartAsync ({}/5)", _tries);
                _tries += 1;
            }

        _tries = 0;
    }

    private async Task InnerStartAsync(CancellationToken cancellationToken)
    {
        await messenger.ConnectAsync();

        messenger.Message.OnMessage += async message =>
        {
            // await bot.SendTextMessageAsync(config.GetTelegramChatId(), MessageHelper.ToCombinedHtml(message),
            //     parseMode: ParseMode.Html, cancellationToken: cancellationToken);
        };

        await bot.SetMyCommandsAsync([
            // new BotCommand
            // {
            //     Command = "online",
            //     Description = "查询服务器在线人数。/online <服务器名>"
            // },
            // new BotCommand
            // {
            //     Command = "time",
            //     Description = "查询服务器世界时间。/time <服务器名> [世界名] [查询类型]"
            // },
            new BotCommand
            {
                Command = "meow",
                Description = "喵~"
            }
        ], cancellationToken: cancellationToken);

        await base.StartAsync(cancellationToken);

        await bot.SendTextMessageAsync(config.GetTelegramChatId(), $"{config.GetName()} 适配器启动了！",
            cancellationToken: cancellationToken);
        // await catMessenger.Message.PublishAsync(new ConnectorMessage
        // {
        //     Content = new TextMessage
        //     {
        //         Text = "适配器启动了！"
        //     }
        // });
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        // await catMessenger.Message.PublishAsync(new ConnectorMessage
        // {
        //     Content = new TextMessage
        //     {
        //         Text = "适配器关闭了！"
        //     }
        // });
        await bot.SendTextMessageAsync(config.GetTelegramChatId(), $"{config.GetName()} 适配器关闭了！",
            cancellationToken: cancellationToken);
        await messenger.DisconnectAsync();
        await base.StopAsync(cancellationToken);
    }
}