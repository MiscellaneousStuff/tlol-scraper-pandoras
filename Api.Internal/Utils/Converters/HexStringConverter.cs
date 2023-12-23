using System.ComponentModel;
using System.Globalization;

namespace Api.Internal.Utils.Converters;

internal class HexStringConverter : TypeConverter
{
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string hexString && hexString.StartsWith("0x") && int.TryParse(hexString.AsSpan(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var intValue))
        {
            return intValue;
        }

        return base.ConvertFrom(context, culture, value);
    }
}