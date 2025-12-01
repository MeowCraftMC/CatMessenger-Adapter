using CatMessenger.Core;
using CatMessenger.Telegram.Config;
using CatMessenger.Telegram.Utilities;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CatMessenger.Telegram.Bot;

public class UpdateHandler(
    ILogger<UpdateHandler> logger,
    ConfigProvider config,
    ITelegramBotClient bot,
    Messenger messenger)
    : IUpdateHandler
{
    private string? Id { get; set; }

    private static Random Random { get; } = new();

    private static string[] Meow { get; } =
    [
        "捕捉小猫猫~",
        "喵？喵！",
        "喵喵喵~",
        "Meow~",
        "喵呜~",
        "喵～～～",
        "喵嗷！",
        "汪~？",
        "犬科动物什么时候才能站起来！"
    ];

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                await OnMessage(update.Message, update.Type);
                return;
            case UpdateType.EditedMessage:
                await OnMessage(update.EditedMessage, update.Type);
                return;
            case UpdateType.ChannelPost:
                await OnMessage(update.ChannelPost, update.Type);
                return;
            case UpdateType.EditedChannelPost:
                await OnMessage(update.EditedChannelPost, update.Type);
                return;
        }
    }

    public Task HandleErrorAsync(ITelegramBotClient _, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogWarning(exception, "Error!");
        return Task.CompletedTask;
    }

    private async Task OnMessage(Message? message, UpdateType type)
    {
        if (message == null)
        {
            return;
        }
        
        if (message.Chat.Id != config.GetTelegramChatId())
        {
            return;
        }

        if (type == UpdateType.Message && message.Text is not null)
        {
            var args = message.Text.Split(' ');
            if (args[0].StartsWith('/'))
            {
                await OnCommand(message, args[0], args);
                return;
            }
        }
        
        var content = UpdateMessageHelper.CreateContentFromMessage(message, message.EditDate != null);
        var sender = UpdateMessageHelper.CreatePlayerFromSender(message);

        if (config.IsDebug() && logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Telegram message: {Message}", message.ToString());
        }

        await messenger.Message.PublishAsync(new Core.Model.Message(config.GetName(), content, sender));
    }

    private async Task OnCommand(Message message, string command, params string[] args)
    {
        if (command.StartsWith("/meow"))
        {
            await bot.SendMessage(message.Chat.Id, Meow[Random.Next(Meow.Length)],
                replyParameters: new ReplyParameters
                {
                    MessageId = message.MessageId
                });
        }
    }

    // public async Task OnCommand(Message message, string command, params string[] args)
    // {
    //     Id ??= (await bot.GetMeAsync()).Username!;
    //
    //     if (command.Contains('@'))
    //     {
    //         var sp = command.Split('@');
    //         if (sp[1] != Id)
    //         {
    //             return;
    //         }
    //     }
    //
    //     if (command.StartsWith("online"))
    //     {
    //         if (args.Length != 1)
    //         {
    //             await bot.SendTextMessageAsync(message.Chat.Id, "用法不正确！\n/online <服务器名>",
    //                 replyToMessageId: message.MessageId);
    //             return;
    //         }
    //
    //         await connector.Publish(new ConnectorCommand
    //         {
    //             Command = ConnectorCommand.EnumCommand.QueryOnline,
    //             Sender = config.GetId(),
    //             Callback = args[0],
    //             ReplyTo = message.MessageId
    //         });
    //         await bot.SendTextMessageAsync(message.Chat.Id, "查询中，请稍候。",
    //             replyToMessageId: message.MessageId);
    //     }
    //     else if (command.StartsWith("time"))
    //     {
    //         if (args.Length is > 3 or < 1)
    //         {
    //             await bot.SendTextMessageAsync(message.Chat.Id, "用法不正确！\n/time <服务器名> [世界名] [查询类型]",
    //                 replyToMessageId: message.MessageId);
    //             return;
    //         }
    //
    //         var world = args.Length >= 2 ? args[1] : "world";
    //         var typeStr = args.Length >= 3 ? args[2] : "DayTime";
    //
    //         await connector.Publish(new ConnectorCommand
    //         {
    //             Command = ConnectorCommand.EnumCommand.QueryWorldTime,
    //             Sender = config.GetId(),
    //             Callback = args[0],
    //             ReplyTo = message.MessageId,
    //             Arguments = [world, typeStr]
    //         });
    //         await bot.SendTextMessageAsync(message.Chat.Id, "查询中，请稍候。",
    //             replyToMessageId: message.MessageId);
    //     }
    //     else if (command.StartsWith("meow"))
    //     {
    //         await bot.SendTextMessageAsync(message.Chat.Id, Meow[Random.Next(Meow.Length)],
    //             replyToMessageId: message.MessageId);
    //     }
    // }
}