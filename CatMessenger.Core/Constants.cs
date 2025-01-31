using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatMessenger.Core;

public static class Constants
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}