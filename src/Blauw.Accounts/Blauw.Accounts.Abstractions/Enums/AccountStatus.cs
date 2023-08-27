using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blauw.Accounts.Abstractions.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum AccountStatus
{
    Active,
    Inactive,
    Suspended,
    Closed,
    Pending,
}
