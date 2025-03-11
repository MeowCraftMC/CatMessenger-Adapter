using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component.Serializer;

public class StringColorSerializer : JsonConverter<ComponentColor>
{
    public override ComponentColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return ComponentColor.From(value);
    }

    public override void Write(Utf8JsonWriter writer, ComponentColor value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}