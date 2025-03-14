using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Styling;
using System.Globalization;

namespace AvpMediaPlayer.UI.Converters
{
    public class PathIconConverter : IValueConverter
    {
        private static Dictionary<string, StreamGeometry> _cache = [];
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var key = $"{value}";
            if (_cache.TryGetValue(key, out var icon))
                return icon;

            if (Application.Current?.TryGetResource(key, ThemeVariant.Dark, out var temp) == true)
                return _cache[key] = (StreamGeometry)temp!;

            return default;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
