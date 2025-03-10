using CatMessenger.Core.Component;
using CatMessenger.Core.Util;

namespace CatMessenger.Core.Model;

public class Message(string platform, string content, Player? sender = null)
{
    public Message(string platform, AbstractComponent content, Player? sender = null)
        : this(platform, JsonHelper.Serialize(content), sender)
    {
    }

    public string Platform { get; set; } = platform;

    public Player? Sender { get; set; } = sender;

    public string Content { get; set; } = content;

    public DateTime Time { get; set; } = DateTime.Now;

    public AbstractComponent GetContent()
    {
        // Todo: not support translatable.
        return JsonHelper.Deserialize<TextComponent>(content) ?? new EmptyComponent();
    }
}