using CatMessenger.Core.Component.Enums;

namespace CatMessenger.Core.Component;

public class TextComponent(string text) : AbstractComponent
{
    public override ComponentType? Type { get; set; } = ComponentType.Text;

    public string Text { get; set; } = text;

    public override string GetString()
    {
        return Text;
    }
}