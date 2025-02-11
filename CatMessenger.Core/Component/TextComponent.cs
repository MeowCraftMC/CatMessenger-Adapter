using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class TextComponent : AbstractComponent
{
    public override ComponentType? Type { get; set; } = ComponentType.Text;

    public required string Text { get; set; } = string.Empty;
}