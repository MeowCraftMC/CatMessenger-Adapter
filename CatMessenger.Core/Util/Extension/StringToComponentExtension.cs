using System.Text.Json.Nodes;
using CatMessenger.Core.Component;
using NLog;

namespace CatMessenger.Core.Util.Extension;

public static class StringToComponentExtension
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    
    public static AbstractComponent ToComponent(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return new EmptyComponent();

        try
        {
            var node = JsonHelper.Deserialize<JsonNode>(str);
            return ComponentJsonSerializer.FromJson(node);
        }
        catch (Exception ex)
        {
            Logger.Error(ex, "Error while handling: {}", str);
            return new EmptyComponent();
        }
    }
}