using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Shadcn.Maui.Core;

public class StringDivisionToDoubleTypeConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
          => sourceType == typeof(string);

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
        => false;

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        var strValue = value?.ToString();

        if (strValue == null)
            return null;

        strValue = strValue.Replace(" ", "");

        if (strValue.Contains('/'))
        {
            var parts = strValue.Split('/');
            if (parts.Length == 2)
            {
                if (double.TryParse(parts[0], out var dividend) && double.TryParse(parts[1], out var divisor))
                {
                    return dividend / divisor;
                }
            }
            else
            {
                throw new ArgumentException("The string must contain only one division symbol.");
            }
        }
        
        if (double.TryParse(strValue, out var result))
        {
            return result;
        }

        throw new ArgumentException("The string must be a double number");
    }
}
