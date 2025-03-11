using System.Text.Json.Serialization;
using CatMessenger.Core.Component;
using CatMessenger.Core.Util;

namespace CatMessenger.Core.Model;

public class Message
{
    [JsonConstructor]
    public Message()
    {
    }

    public Message(string platform, AbstractComponent content, Player? sender = null)
        : this(platform, JsonHelper.Serialize(content), sender)
    {
    }

    public Message(string platform, string content, Player? sender = null)
    {
        Platform = platform;
        Sender = sender;
        Content = content;
    }

    public string Platform { get; set; }

    public Player? Sender { get; set; }

    public string Content { get; set; }

    public DateTime Time { get; set; } = DateTime.Now;
}