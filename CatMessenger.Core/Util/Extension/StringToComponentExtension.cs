using System.Text.Json.Nodes;
using CatMessenger.Core.Component;

namespace CatMessenger.Core.Util.Extension;

public static class StringToComponentExtension
{
    public static AbstractComponent ToComponent(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return new EmptyComponent();

        var node = JsonHelper.Deserialize<JsonNode>(str);
        return ComponentJsonSerializer.FromJson(node);
    }
}