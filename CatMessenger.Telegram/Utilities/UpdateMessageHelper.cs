using CatMessenger.Core.Component;
using CatMessenger.Core.Model;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Message = Telegram.Bot.Types.Message;

namespace CatMessenger.Telegram.Utilities;

public class UpdateMessageHelper
{
    private static AbstractComponent CreateUser(User? user)
    {
        if (user is null) return new EmptyComponent();

        var name = user.FirstName;
        if (!string.IsNullOrWhiteSpace(user.LastName)) name += $" {user.LastName}";
        var from = new TextComponent(name)
        {
            Color = ComponentColor.Aqua
        };

        if (!string.IsNullOrWhiteSpace(user.Username))
            from.HoverEvent = new HoverEvent(new TextComponent($"@{user.Username}"));

        return from;
    }

    private static AbstractComponent CreateChat(Chat? chat)
    {
        if (chat is null) return new EmptyComponent();

        switch (chat.Type)
        {
            case ChatType.Channel or ChatType.Supergroup or ChatType.Group:
            {
                return new TextComponent(chat.Title!)
                {
                    Bold = true
                };
            }
            case ChatType.Private or ChatType.Sender:
            {
                var text = string.Empty;
                if (!string.IsNullOrWhiteSpace(chat.FirstName)) text += $"{chat.FirstName}";

                if (!string.IsNullOrWhiteSpace(chat.LastName))
                {
                    if (!string.IsNullOrWhiteSpace(text)) text += " ";

                    text += $"{chat.LastName}";
                }

                var c = new TextComponent(text)
                {
                    Color = ComponentColor.Aqua
                };

                if (!string.IsNullOrWhiteSpace(chat.Username))
                    c.HoverEvent = new HoverEvent(new TextComponent($"@{chat.Username}"));

                return c;
            }
        }

        return new EmptyComponent();
    }

    private static AbstractComponent DecorateByEntity(AbstractComponent component, MessageEntity entity,
        bool disableHover = false)
    {
        switch (entity.Type)
        {
            case MessageEntityType.Mention:
            case MessageEntityType.TextMention:
            {
                component.Color = ComponentColor.Blue;
                component.Underlined = true;

                var hover = CreateUser(entity.User);
                if (disableHover)
                {
                    var h = new TextComponent(" (");
                    h.Extra.Add(hover);
                    h.Extra.Add(new TextComponent(") "));
                    component.Extra.Add(h);
                }
                else
                {
                    component.HoverEvent = new HoverEvent(hover);
                }

                break;
            }
            case MessageEntityType.Url:
            case MessageEntityType.TextLink:
            {
                component.Color = ComponentColor.Blue;
                component.Underlined = true;
                if (entity.Url is not null)
                {
                    component.ClickEvent = new ClickEvent(ClickAction.CopyToClipboard, entity.Url);

                    if (disableHover)
                        component.Extra.Add(new TextComponent($" ({entity.Url}) "));
                    else
                        component.HoverEvent = new HoverEvent(new TextComponent(entity.Url));
                }

                break;
            }
            case MessageEntityType.BotCommand:
            case MessageEntityType.PhoneNumber:
            case MessageEntityType.Hashtag:
            case MessageEntityType.Email:
            {
                component.Color = ComponentColor.Blue;
                break;
            }
            case MessageEntityType.Bold:
                component.Bold = true;
                break;
            case MessageEntityType.Italic:
                component.Italic = true;
                break;
            case MessageEntityType.Underline:
                component.Underlined = true;
                break;
            case MessageEntityType.Strikethrough:
                component.Strikethrough = true;
                break;
            case MessageEntityType.Spoiler:
            {
                component.Obfuscated = true;

                if (disableHover)
                    component.Extra.Add(new TextComponent($" ({component}) "));
                else
                    component.HoverEvent = new HoverEvent(new TextComponent(component.ToString()));

                break;
            }
            case MessageEntityType.Code:
            case MessageEntityType.Pre:
            case MessageEntityType.Cashtag:
            case MessageEntityType.CustomEmoji:
            default:
                break;
        }

        return component;
    }

    private static List<AbstractComponent> CreateStyledText(string text, MessageEntity[] entities,
        bool disableHover = false)
    {
        if (entities.Length == 0) return [new TextComponent(text)];

        var result = new List<AbstractComponent>();
        var bufferedEntitySet = new HashSet<MessageEntity>();
        var bufferStartCursor = 0;
        for (var bufferEndCursor = 0; bufferEndCursor <= text.Length; bufferEndCursor++)
        {
            var currentEntities = new HashSet<MessageEntity>();
            foreach (var e in entities)
                if (e.Offset <= bufferEndCursor && e.Offset + e.Length > bufferEndCursor)
                    currentEntities.Add(e);

            if (!bufferedEntitySet.SetEquals(currentEntities) || bufferEndCursor == text.Length)
            {
                var prevText = text.Substring(bufferStartCursor, bufferEndCursor - bufferStartCursor);
                if (!string.IsNullOrEmpty(prevText))
                {
                    AbstractComponent c = new TextComponent(prevText);
                    foreach (var e in bufferedEntitySet) c = DecorateByEntity(c, e, disableHover);
                    result.Add(c);
                }

                bufferStartCursor = bufferEndCursor;
            }

            bufferedEntitySet = currentEntities;
        }

        return result;
    }

    private static AbstractComponent CreateSticker(Sticker sticker)
    {
        return new TextComponent($"[贴纸 {sticker.Emoji}] ")
        {
            Color = ComponentColor.Green,
            HoverEvent = new HoverEvent(new TextComponent($"来自贴纸包 {sticker.SetName}"))
        };
    }

    public static AbstractComponent CreateContentFromMessage(Message message, bool edited = false)
    {
        var contentComponent = new EmptyComponent();

        if (edited)
        {
            var edit = new TextComponent("[已编辑] ")
            {
                Color = ComponentColor.LightPurple
            };
            contentComponent.Extra.Add(edit);
        }

        if (message.ReplyToMessage != null)
        {
            var reply = message.ReplyToMessage;

            var hover = new EmptyComponent();
            hover.Extra.AddRange(CreateStyledText(reply.Caption ?? reply.Text!,
                reply.CaptionEntities ?? reply.Entities ?? []));

            var hoverEvent = new HoverEvent(hover);

            var replyComponent = new TextComponent("[回复：")
            {
                Color = ComponentColor.LightPurple,
                HoverEvent = hoverEvent
            };

            var from = CreateUser(reply.From);
            from.Color = ComponentColor.Aqua;
            from.HoverEvent = hoverEvent;
            replyComponent.Extra.Add(from);

            replyComponent.Extra.Add(new TextComponent("] ")
            {
                Color = ComponentColor.LightPurple,
                HoverEvent = hoverEvent
            });

            contentComponent.Extra.Add(replyComponent);
        }

        if (message.ForwardFrom != null)
        {
            var forwardFrom = message.ForwardFrom;

            var forwardComponent = new TextComponent("[转发自 ")
            {
                Color = ComponentColor.LightPurple
            };
            forwardComponent.Extra.Add(CreateUser(forwardFrom));
            forwardComponent.Extra.Add(new TextComponent("] "));

            contentComponent.Extra.Add(forwardComponent);
        }

        if (message.ForwardFromChat != null)
        {
            var forwardFrom = message.ForwardFromChat;

            var forwardComponent = new TextComponent("[转发自 ")
            {
                Color = ComponentColor.LightPurple
            };
            forwardComponent.Extra.Add(CreateChat(forwardFrom));
            forwardComponent.Extra.Add(new TextComponent("] "));

            contentComponent.Extra.Add(forwardComponent);
        }

        if (message.Photo is { Length: > 0 })
            contentComponent.Extra.Add(new TextComponent("[图片] ")
            {
                Color = ComponentColor.Green
            });

        if (message.Sticker != null) contentComponent.Extra.Add(CreateSticker(message.Sticker));

        if (message.Document != null)
            contentComponent.Extra.Add(new TextComponent($"[文件 {message.Document.FileName}] ")
            {
                Color = ComponentColor.Green
            });

        if (message.Voice != null)
            contentComponent.Extra.Add(new TextComponent($"[语音 {message.Voice.Duration}秒] ")
            {
                Color = ComponentColor.Green
            });

        if (message.Audio != null)
            contentComponent.Extra.Add(new TextComponent($"[音频 {message.Voice.Duration}秒] ")
            {
                Color = ComponentColor.Green
            });

        if (message.Video != null)
            contentComponent.Extra.Add(new TextComponent($"[视频 {message.Voice.Duration}秒] ")
            {
                Color = ComponentColor.Green
            });

        contentComponent.Extra.AddRange(CreateStyledText(message.Caption ?? message.Text ?? "",
            message.CaptionEntities ?? message.Entities ?? []));

        return contentComponent;
    }

    public static Player? CreatePlayerFromSender(Message message)
    {
        if (message.From is null) return null;

        return new Player(message.From.Username ?? message.From.Id.ToString(),
            message.From.FirstName + (message.From.LastName != null ? $" {message.From.LastName}" : string.Empty));
    }

    private static AbstractComponent FromUnsupported(Update update)
    {
        return new TextComponent($"[不支持的消息 {update.Type}] ")
        {
            Color = ComponentColor.Red
        };
    }

    private static AbstractComponent FromUnknown(Update update)
    {
        return new TextComponent("[未知消息] ")
        {
            Color = ComponentColor.Red
        };
    }
}