using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Blauw.Common.Abstractions.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Currency
{
    EUR,
    USD,
    GBP,
}