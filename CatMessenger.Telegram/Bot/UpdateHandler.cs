using CatMessenger.Core;
using CatMessenger.Telegram.Config;
using CatMessenger.Telegram.Utilities;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CatMessenger.Telegram.Bot;

public class UpdateHandler(
    ILogger<UpdateHandler> logger,
    ConfigProvider config,
    ITelegramBotClient bot,
    Messenger messenger)
    : IUpdateHandler
{
    /// <summary>
    ///     TimeZone: UTC
    /// </summary>
    private DateTime StartTime { get; } = DateTime.Now.ToUniversalTime();

    private string? Id { get; set; }

    private static Random Random { get; } = new();

    private static string[] Meow { get; } =
    [
        "捕捉小猫猫~",
        "喵？喵！",
        "喵喵喵~",
        "Meow~",
        "犬科动物什么时候才能站起来！"
    ];

    public async Task HandleUpdateAsync(ITelegramBotClient _, Update update, CancellationToken cancellationToken)
    {
        var message = FilterMessage(update);

        if (message is null) return;

        // if (update.Type == UpdateType.Message
        //     && update.Message!.Type == MessageType.Text
        //     && update.Message.Text!.StartsWith('/'))
        // {
        //     var command = update.Message.Text[1..].Split(" ");
        //     logger.LogInformation("Telegram command: {Command}", update.Message.Text);
        //     await OnCommand(update.Message, command[0], command[1..]);
        //     return;
        // }

        var content = UpdateMessageHelper.CreateContentFromMessage(message, message.EditDate != null);
        var sender = UpdateMessageHelper.CreatePlayerFromSender(message);

        if (config.IsDebug()) logger.LogInformation("Telegram message: {Message}", message.ToString());

        await messenger.Message.PublishAsync(new Core.Model.Message(config.GetName(), content, sender));
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
        CancellationToken cancellationToken)
    {
        logger.LogWarning(exception, "Error!");
        return Task.CompletedTask;
    }

    private Message? FilterMessage(Update update)
    {
        if (update.Message != null
            && update.Message.Chat.Id == config.GetTelegramChatId()
            && StartTime.CompareTo(update.Message?.Date) != 1)
            return update.Message;

        if (update.ChannelPost != null
            && update.ChannelPost.Chat.Id == config.GetTelegramChatId()
            && StartTime.CompareTo(update.ChannelPost?.Date) != 1)
            return update.ChannelPost;

        if (update.EditedMessage != null
            && update.EditedMessage.Chat.Id == config.GetTelegramChatId()
            && StartTime.CompareTo(update.EditedMessage?.EditDate) != 1)
            return update.EditedMessage;

        if (update.EditedChannelPost != null
            && update.EditedChannelPost.Chat.Id == config.GetTelegramChatId()
            && StartTime.CompareTo(update.EditedChannelPost?.EditDate) != 1)
            return update.EditedChannelPost;

        return null;
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