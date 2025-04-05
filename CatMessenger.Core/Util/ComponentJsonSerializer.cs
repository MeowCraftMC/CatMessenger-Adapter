using System.Text.Json.Nodes;
using CatMessenger.Core.Component;

namespace CatMessenger.Core.Util;

public static class ComponentJsonSerializer
{
    public static AbstractComponent FromJson(JsonNode? n)
    {
        return n switch
        {
            JsonValue value => new TextComponent(value.ToString()),
            JsonArray array => new EmptyComponent { Extra = [..FromJson(array)] },
            JsonObject obj => FromJson(obj),
            _ => new EmptyComponent()
        };
    }

    public static List<AbstractComponent> FromJson(JsonArray a)
    {
        return a.Select(FromJson).ToList();
    }

    public static AbstractComponent FromJson(JsonObject o)
    {
        AbstractComponent result;

        var type = o["type"]?.ToString();
        if (type == "text" || o.ContainsKey("text"))
        {
            var text = o["text"]!.ToString();
            result = new TextComponent(text);
        }
        else if (type == "translatable" || o.ContainsKey("translate"))
        {
            var translate = o["translate"]!.ToString();
            var fallback = o["fallback"]?.ToString();
            var t = new TranslatableComponent
            {
                Translate = translate,
                Fallback = fallback
            };

            if (o.ContainsKey("with")) t.With = o["with"]!.AsArray().Select(FromJson).ToList();

            result = t;
        }
        else
        {
            // Todo: other type
            result = new EmptyComponent();
        }

        var color = o["color"]?.AsValue();
        if (color is not null)
            if (color.TryGetValue<string>(out var v))
                result.Color = ComponentColor.From(v);

        var shadowColor = o["shadow_color"]?.AsArray();
        if (shadowColor is not null && shadowColor.Count == 4)
        {
            var aV = shadowColor[0]?.AsValue();
            var rV = shadowColor[1]?.AsValue();
            var gV = shadowColor[2]?.AsValue();
            var bV = shadowColor[3]?.AsValue();
            if (aV is not null && aV.TryGetValue<float>(out var a)
                               && rV is not null && rV.TryGetValue<float>(out var r)
                               && gV is not null && gV.TryGetValue<float>(out var g)
                               && bV is not null && bV.TryGetValue<float>(out var b))
                result.ShadowColor = ComponentColor.From(a, r, g, b);
        }

        var font = o["font"]?.AsValue();
        if (font is not null)
            if (font.TryGetValue<string>(out var v))
                result.Font = v;

        var bold = o["bold"]?.AsValue();
        if (bold is not null)
            if (bold.TryGetValue<bool>(out var v))
                result.Bold = v;

        var italic = o["italic"]?.AsValue();
        if (italic is not null)
            if (italic.TryGetValue<bool>(out var v))
                result.Italic = v;

        var underlined = o["underlined"]?.AsValue();
        if (underlined is not null)
            if (underlined.TryGetValue<bool>(out var v))
                result.Underlined = v;

        var strikethrough = o["strikethrough"]?.AsValue();
        if (strikethrough is not null)
            if (strikethrough.TryGetValue<bool>(out var v))
                result.Strikethrough = v;

        var obfuscated = o["obfuscated"]?.AsValue();
        if (obfuscated is not null)
            if (obfuscated.TryGetValue<bool>(out var v))
                result.Obfuscated = v;

        var insertion = o["insertion"]?.AsValue();
        if (insertion is not null)
            if (insertion.TryGetValue<string>(out var v))
                result.Insertion = v;

        var hoverEvent = o["hoverEvent"]?.AsObject();
        if (hoverEvent is not null)
        {
            var action = hoverEvent["action"]?.AsValue();
            var contents = hoverEvent["contents"]?.AsObject();
            if (action is not null && contents is not null)
            {
                var r = FromJson(contents);
                result.HoverEvent = new HoverEvent(r);
            }
        }

        var clickEvent = o["clickEvent"]?.AsObject();
        if (clickEvent is not null)
        {
            var action = clickEvent["action"]?.AsValue();
            var value = clickEvent["value"]?.AsValue();
            if (action is not null && value is not null)
                if (action.TryGetValue<string>(out var a) && value.TryGetValue<string>(out var v))
                {
                    var ac = Enum.Parse<ClickAction>(a.Replace("_", ""), true);
                    result.ClickEvent = new ClickEvent(ac, v);
                }
        }

        var extra = o["extra"]?.AsArray();
        if (extra is not null) result.Extra = FromJson(extra);

        return result;
    }
}