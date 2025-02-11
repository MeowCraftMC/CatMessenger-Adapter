using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component.Serializer;

public class IntPackedColorSerializer : JsonConverter<ComponentColor>
{
    public override ComponentColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
            {
                var value = reader.GetString();
                if (value == null) return null;
                return int.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var packed)
                    ? new ComponentColor(packed)
                    : null;
            }
            case JsonTokenType.StartArray when reader.TryGetSingle(out var a)
                                               && reader.TryGetSingle(out var r)
                                               && reader.TryGetSingle(out var g)
                                               && reader.TryGetSingle(out var b):
            {
                var alpha = (int)(a * 255);
                var red = (int)(r * 255);
                var green = (int)(g * 255);
                var blue = (int)(b * 255);
                var packed = (alpha << 24) + (red << 16) + (green << 8) + blue;
                return new ComponentColor(packed);
            }
            default:
                return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, ComponentColor value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value.Hex);
    }
}