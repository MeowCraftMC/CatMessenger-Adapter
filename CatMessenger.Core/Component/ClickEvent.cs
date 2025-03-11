using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component;

public class ClickEvent
{
    [JsonConstructor]
    public ClickEvent()
    {
    }

    public ClickEvent(ClickAction action, string value)
    {
        Action = action;
        Value = value;
    }

    [JsonConverter(typeof(JsonStringEnumConverter<ClickAction>))]
    public ClickAction Action { get; set; }

    public string Value { get; set; }
}