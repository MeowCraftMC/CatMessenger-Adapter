namespace CatMessenger.Core.Component;

public interface IStyledComponent
{
    ComponentColor? Color { get; set; }

    ComponentColor? ShadowColor { get; set; }

    string? Font { get; set; }

    bool Bold { get; set; }

    bool Italic { get; set; }

    bool Underlined { get; set; }

    bool Strikethrough { get; set; }

    bool Obfuscated { get; set; }

    string? Insertion { get; set; }

    /// <summary>
    ///     clickEvent 1.21.5-
    ///     click_event 1.21.5+
    /// </summary>
    ClickEvent? ClickEvent { get; set; }

    /// <summary>
    ///     hoverEvent 1.21.5-
    ///     hover_event 1.21.5+
    /// </summary>
    HoverEvent? HoverEvent { get; set; }
}