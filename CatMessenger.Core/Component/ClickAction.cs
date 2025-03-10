using System.Runtime.Serialization;

namespace CatMessenger.Core.Component;

public enum ClickAction
{
    [EnumMember(Value = "open_url")] OpenUrl = 0,
    [EnumMember(Value = "open_file")] OpenFile = 1,
    [EnumMember(Value = "run_command")] RunCommand = 2,

    [EnumMember(Value = "suggest_command")]
    SuggestCommand = 3,
    [EnumMember(Value = "change_page")] ChangePage = 4,

    [EnumMember(Value = "copy_to_clipboard")]
    CopyToClipboard = 5
}