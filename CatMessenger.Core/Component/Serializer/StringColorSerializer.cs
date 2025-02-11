using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component.Serializer;

public class StringColorSerializer : JsonConverter<ComponentColor>
{
    public override ComponentColor? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (value == null) return null;
        if (value.StartsWith('#') && value.Length == 7)
        {
            var hexString = value.Substring(1, 6);
            return int.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var hex)
                ? new ComponentColor(hex)
                : null;
        }

        return ComponentColor.NamedColors.FirstOrDefault(named => value == named.Name);
    }

    public override void Write(Utf8JsonWriter writer, ComponentColor value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}