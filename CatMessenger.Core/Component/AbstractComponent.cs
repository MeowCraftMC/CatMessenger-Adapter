using System.Text.Json.Serialization;
using CatMessenger.Core.Component.Enums;
using CatMessenger.Core.Component.Serializer;

namespace CatMessenger.Core.Component;

public abstract class AbstractComponent : IStyledComponent
{
    public abstract ComponentType? Type { get; set; }

    public List<AbstractComponent> Extra { get; set; } = [];

    [JsonConverter(typeof(StringColorSerializer))]
    public ComponentColor? Color { get; set; }

    [JsonConverter(typeof(IntPackedColorSerializer))]
    public ComponentColor? ShadowColor { get; set; }

    public string? Font { get; set; }
    public bool Bold { get; set; }
    public bool Italic { get; set; }
    public bool Underlined { get; set; }
    public bool Strikethrough { get; set; }
    public bool Obfuscated { get; set; }
    public string? Insertion { get; set; }
    public ClickEvent? ClickEvent { get; set; }
    public HoverEvent? HoverEvent { get; set; }
}