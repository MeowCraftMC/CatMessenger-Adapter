using System.Runtime.Serialization;

namespace CatMessenger.Core.Component;

public enum HoverAction
{
    [EnumMember(Value = "show_text")] ShowText = 0,
    [EnumMember(Value = "show_item")] ShowItem = 1,
    [EnumMember(Value = "show_entity")] ShowEntity = 2
}