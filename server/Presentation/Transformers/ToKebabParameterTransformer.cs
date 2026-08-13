using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Routing;

namespace Presentation.Transformers;

public partial class ToKebabParameterTransformer : IOutboundParameterTransformer
{
    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex KebabCaseGeneratedRegex();

    public string? TransformOutbound(object? value)
    {
        if (value is not string str)
            return null;

#pragma warning disable CA1304
        return KebabCaseGeneratedRegex().Replace(str, "$1-$2").ToLower(System.Globalization.CultureInfo.CurrentCulture);
#pragma warning restore CA1304
    }
}