namespace CatMessenger.Core.Model;

public class Message
{
    public required string Platform { get; set; }

    public Player? Player { get; set; }

    public required string Content { get; set; }

    public required DateTime Time { get; set; } = DateTime.Now;
}