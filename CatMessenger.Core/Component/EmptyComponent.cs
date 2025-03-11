using System.Text.Json.Serialization;
using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class EmptyComponent() : TextComponent("")
{
    [JsonConverter(typeof(JsonStringEnumConverter<ComponentType>))]
    public override ComponentType? Type { get; set; } = ComponentType.Text;
}