using System.ComponentModel;

namespace Domain.Enums;

public enum FileType
{
    [Description("jpg,png,jpeg,gif")]
    Image,

    [Description("mp4,webm,ogg,ogv,avi,mkv,wmv")]
    Video
}