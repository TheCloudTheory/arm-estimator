using System.Text.Json;

namespace ACE_Tests.Reworked;

internal sealed class Shared
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}