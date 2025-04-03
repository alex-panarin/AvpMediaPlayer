using Avalonia.Data.Converters;
using AvpMediaPlayer.Media.Interfaces;
using System.Globalization;

namespace AvpMediaPlayer.UI.Converters
{
    public class DurationConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if(value is IMediaManagement media)
            {
                return $"{media.Position}:{media.Duration}";
            }
            return string.Empty;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
