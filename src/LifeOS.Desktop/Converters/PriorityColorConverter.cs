using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;
using LifeOS.Domain.Tasks;

namespace LifeOS.Desktop.Converters;

public class PriorityColorConverter : IValueConverter
{
    public static readonly PriorityColorConverter Instance = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value switch
        {
            TaskPriority.High => new SolidColorBrush(Color.Parse("#F87171")),
            TaskPriority.Medium => new SolidColorBrush(Color.Parse("#FBBF24")),
            TaskPriority.Low => new SolidColorBrush(Color.Parse("#4ADE80")),
            _ => new SolidColorBrush(Colors.Gray)
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
