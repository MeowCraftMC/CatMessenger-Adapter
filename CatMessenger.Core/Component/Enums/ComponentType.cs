using System.Runtime.Serialization;

namespace CatMessenger.Core.Component.Enums;

public enum ComponentType
{
    [EnumMember(Value = "text")] Text = 0,
    [EnumMember(Value = "translatable")] Translatable = 1,
    [EnumMember(Value = "score")] Score = 2,
    [EnumMember(Value = "nbt")] Nbt = 3,
    [EnumMember(Value = "selector")] Selector = 4,
    [EnumMember(Value = "keybind")] KeyBind = 5
}