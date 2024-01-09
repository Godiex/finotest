using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Domain.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CustomerPlanState
    {
        Active,
        Deactivated
    }
}
