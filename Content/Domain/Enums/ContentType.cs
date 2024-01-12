using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ContentType
    {
        ACCORDION_DETAIL,
        PLANE_TEXT,
    }

}
