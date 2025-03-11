using System.Text.Json.Serialization;
using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class TranslatableComponent : AbstractComponent
{
    [JsonConverter(typeof(JsonStringEnumConverter<ComponentType>))]
    public override ComponentType? Type { get; set; } = ComponentType.Translatable;

    public required string Translate { get; set; } = string.Empty;

    public string? Fallback { get; set; }

    public List<AbstractComponent> With { get; set; } = [];

    public override string GetString()
    {
        return string.Format(Translate, With);
    }
}