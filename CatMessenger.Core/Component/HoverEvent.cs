namespace CatMessenger.Core.Component;

public class HoverEvent(HoverAction action, AbstractComponent contents)
{
    public HoverEvent(AbstractComponent contents) : this(HoverAction.ShowText, contents)
    {
    }

    public HoverAction Action { get; set; } = action;

    // Todo: not support entity nor item now.
    public AbstractComponent Contents { get; set; } = contents;
}