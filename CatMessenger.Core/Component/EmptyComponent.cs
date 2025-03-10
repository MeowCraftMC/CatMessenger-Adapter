using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class EmptyComponent() : TextComponent("")
{
    public override ComponentType? Type { get; set; } = ComponentType.Text;
}