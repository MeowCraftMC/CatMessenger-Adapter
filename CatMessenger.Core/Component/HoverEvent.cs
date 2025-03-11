using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component;

public class HoverEvent
{
    [JsonConstructor]
    public HoverEvent()
    {
    }

    public HoverEvent(AbstractComponent contents) : this(HoverAction.ShowText, contents)
    {
    }

    public HoverEvent(HoverAction action, AbstractComponent contents)
    {
        Action = action;
        Contents = contents;
    }

    [JsonConverter(typeof(JsonStringEnumConverter<HoverAction>))]
    public HoverAction Action { get; set; }

    // Todo: not support entity nor item now.
    public AbstractComponent Contents { get; set; }
}