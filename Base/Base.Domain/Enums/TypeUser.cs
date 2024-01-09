using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TypeUser
    {
        Admin,
        Customer
    }

}
