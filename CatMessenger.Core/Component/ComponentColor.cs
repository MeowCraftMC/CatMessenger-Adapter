namespace CatMessenger.Core.Component;

public class ComponentColor
{
    public static readonly ComponentColor Black = new(0x000000, "black");
    public static readonly ComponentColor DarkBlue = new(0x0000AA, "dark_blue");
    public static readonly ComponentColor DarkGreen = new(0x00AA00, "dark_green");
    public static readonly ComponentColor DarkAqua = new(0x00AAAA, "dark_aqua");
    public static readonly ComponentColor DarkRed = new(0xAA0000, "dark_red");
    public static readonly ComponentColor DarkPurple = new(0xAA00AA, "dark_purple");
    public static readonly ComponentColor Gold = new(0xFFAA00, "gold");
    public static readonly ComponentColor Gray = new(0xAAAAAA, "gray");
    public static readonly ComponentColor DarkGray = new(0x555555, "dark_gray");
    public static readonly ComponentColor Blue = new(0x5555FF, "blue");
    public static readonly ComponentColor Green = new(0x55FF55, "green");
    public static readonly ComponentColor Aqua = new(0x55FFFF, "aqua");
    public static readonly ComponentColor Red = new(0xFF5555, "red");
    public static readonly ComponentColor LightPurple = new(0xFF55FF, "light_purple");
    public static readonly ComponentColor Yellow = new(0xFFFF55, "yellow");
    public static readonly ComponentColor White = new(0xFFFFFF, "white");

    public static readonly List<ComponentColor> NamedColors = [];

    public ComponentColor(int hexColor)
    {
        Hex = hexColor;
    }

    private ComponentColor(int hex, string name)
    {
        Hex = hex;
        Name = name;
        NamedColors.Add(this);
    }

    public string? Name { get; }

    public int Hex { get; }

    public override string ToString()
    {
        return Name ?? $"#{Hex:X}";
    }

    public override bool Equals(object? obj)
    {
        if (obj is ComponentColor color) return Equals(color);

        return false;
    }

    protected bool Equals(ComponentColor other)
    {
        return Name == other.Name && Hex == other.Hex;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Hex);
    }
}