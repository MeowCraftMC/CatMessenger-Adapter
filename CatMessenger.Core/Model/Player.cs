using System.Text.Json.Serialization;
using CatMessenger.Core.Util;

namespace CatMessenger.Core.Model;

public class Player
{
    [JsonConstructor]
    public Player()
    {
    }

    public Player(string id, string name)
    {
        Id = id;
        Name = JsonHelper.Serialize(name);
    }
    
    public string Id { get; set; }

    public Guid? Uuid { get; set; }

    public string? Name { get; set; }

    public string? Prefix { get; set; }

    public string? Suffix { get; set; }
}