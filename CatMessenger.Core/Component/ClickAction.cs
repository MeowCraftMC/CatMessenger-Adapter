using System.Text.Json.Serialization;

namespace CatMessenger.Core.Component;

public enum ClickAction
{
    [JsonStringEnumMemberName("open_url")] OpenUrl = 0,

    [JsonStringEnumMemberName("open_file")]
    OpenFile = 1,

    [JsonStringEnumMemberName("run_command")]
    RunCommand = 2,

    [JsonStringEnumMemberName("suggest_command")]
    SuggestCommand = 3,

    [JsonStringEnumMemberName("change_page")]
    ChangePage = 4,

    [JsonStringEnumMemberName("copy_to_clipboard")]
    CopyToClipboard = 5
}