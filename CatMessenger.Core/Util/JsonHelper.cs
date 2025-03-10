using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatMessenger.Core.Util;

public class JsonHelper
{
    public static JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public static string Serialize(object o)
    {
        return JsonSerializer.Serialize(o, JsonSerializerOptions);
    }

    public static T? Deserialize<T>(string o)
    {
        return JsonSerializer.Deserialize<T>(o);
    }
}