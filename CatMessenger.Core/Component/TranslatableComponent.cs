using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class TranslatableComponent : AbstractComponent
{
    public override ComponentType? Type { get; set; } = ComponentType.Translatable;

    public required string Translate { get; set; } = string.Empty;

    public string? Fallback { get; set; }

    public List<AbstractComponent> With { get; set; } = [];
}