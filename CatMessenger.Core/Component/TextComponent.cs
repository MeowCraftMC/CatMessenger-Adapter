using System.Text.Json.Serialization;
using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class TextComponent : AbstractComponent
{
    [JsonConstructor]
    public TextComponent()
    {
    }

    public TextComponent(string text)
    {
        Text = text;
    }

    [JsonConverter(typeof(JsonStringEnumConverter<ComponentType>))]
    public override ComponentType? Type { get; set; } = ComponentType.Text;

    public string Text { get; set; }

    public override string GetString()
    {
        return Text;
    }
}