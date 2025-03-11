using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component.Serializer;

public class TextComponentSerializer : JsonConverter<TextComponent>
{
    private static readonly JsonConverter<TextComponent> _defaultConverter =
        (JsonConverter<TextComponent>)JsonSerializerOptions.Default.GetConverter(typeof(TextComponent));

    public override TextComponent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String) return new TextComponent(reader.GetString());

        if (reader.TokenType == JsonTokenType.StartArray)
        {
            reader.Skip();
            var result = new EmptyComponent();
            while (reader.TokenType != JsonTokenType.EndArray)
            {
                var r = Read(ref reader, typeToConvert, options);
                if (r != null) result.Extra.Add(r);
            }

            reader.Skip();
            return result;
        }

        return JsonSerializer.Deserialize<TextComponent>(ref reader);

        // As an object.
        return _defaultConverter.Read(ref reader, typeToConvert, options);
    }

    public override void Write(Utf8JsonWriter writer, TextComponent value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value);
        // _defaultConverter.Write(writer, value, options);
    }
}