using System.Text;
using System.Text.Encodings.Web;
using CatMessenger.Core.Component;
using CatMessenger.Core.Model;
using CatMessenger.Core.Util.Extension;

namespace CatMessenger.Telegram.Utilities;

public class MessageHelper
{
    public static string ToCombinedHtml(Message message)
    {
        if (message.Sender is null)
            return $"""
                    〔{message.Platform}〕{ToHtml(message.Content.ToComponent())}
                    """;

        return $"""
                〔{message.Platform}〕<b>{ToHtml(message.Sender)}</b>：
                {ToHtml(message.Content.ToComponent())}
                """;
    }

    private static string ToHtml(AbstractComponent? component)
    {
        if (component is null) return string.Empty;

        var builder = new StringBuilder();
        builder.Append(HtmlEncoder.Default.Encode(component.ToString()));

        foreach (var e in component.Extra) builder.Append(ToHtml(e));

        if (component.HoverEvent != null)
        {
            // Todo: not support hover
        }

        if (component.ClickEvent != null)
        {
            // Todo: not support click
        }

        if (component.Color != null)
        {
            // Todo: not support color
        }

        if (component.Bold)
        {
            builder.Insert(0, "<b>");
            builder.Append("</b>");
        }

        if (component.Italic)
        {
            builder.Insert(0, "<i>");
            builder.Append("</i>");
        }

        if (component.Underlined)
        {
            builder.Insert(0, "<u>");
            builder.Append("</u>");
        }

        if (component.Strikethrough)
        {
            builder.Insert(0, "<del>");
            builder.Append("</del>");
        }

        if (component.Obfuscated)
        {
            builder.Insert(0, "<tg-spoiler>");
            builder.Append("</tg-spoiler>");
        }

        return builder.ToString();
    }

    private static string ToHtml(Player player)
    {
        return player.Name is not null ? ToHtml(player.Name.ToComponent()) : player.Id;
    }
}