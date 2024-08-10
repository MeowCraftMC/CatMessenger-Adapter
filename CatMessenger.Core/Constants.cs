using Newtonsoft.Json;

namespace CatMessenger.Core;

public class Constants
{
    public static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore
    };
}