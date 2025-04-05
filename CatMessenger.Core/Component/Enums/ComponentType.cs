using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component.Enums;

public enum ComponentType
{
    [JsonStringEnumMemberName("text")] Text = 0,

    [JsonStringEnumMemberName("translatable")]
    Translatable = 1,
    [JsonStringEnumMemberName("score")] Score = 2,
    [JsonStringEnumMemberName("nbt")] Nbt = 3,
    [JsonStringEnumMemberName("selector")] Selector = 4,
    [JsonStringEnumMemberName("keybind")] KeyBind = 5
}