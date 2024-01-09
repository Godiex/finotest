using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContentType
    {
        SECTION,
        PLANE_TEXT,
    }

}
