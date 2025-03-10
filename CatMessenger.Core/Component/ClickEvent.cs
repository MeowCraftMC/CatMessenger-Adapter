namespace CatMessenger.Core.Component;

public class ClickEvent(ClickAction action, string value)
{
    public ClickAction Action { get; set; } = action;

    public string Value { get; set; } = value;
}