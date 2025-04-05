using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component;

public enum HoverAction
{
    [JsonStringEnumMemberName("show_text")]
    ShowText = 0,

    [JsonStringEnumMemberName("show_item")]
    ShowItem = 1,

    [JsonStringEnumMemberName("show_entity")]
    ShowEntity = 2
}