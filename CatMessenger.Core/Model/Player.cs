namespace CatMessenger.Core.Model;

public class Player
{
    public required string Id { get; set; }

    public Guid? Uuid { get; set; }

    public string? Name { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }
}