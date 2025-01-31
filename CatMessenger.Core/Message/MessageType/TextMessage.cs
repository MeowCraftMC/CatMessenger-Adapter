using Newtonsoft.Json.Linq;

namespace CatMessenger.Core.Message.MessageType;

public class TextMessage : AbstractMessage
{
    public string Text { get; set; } = string.Empty;

    public override string GetType()
    {
        return "text";
    }

    public override JObject WriteData()
    {
        var jObject = new JObject();
        jObject["text"] = Text ?? string.Empty;
        return jObject;
    }

    public override void ReadData(JObject jObject)
    {
        Text = jObject.Value<string>("text")!;
    }
}