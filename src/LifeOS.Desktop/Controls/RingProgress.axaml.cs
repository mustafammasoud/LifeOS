using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace LifeOS.Desktop.Controls;

public partial class RingProgress : UserControl
{
    public static readonly StyledProperty<double> PercentageProperty =
        AvaloniaProperty.Register<RingProgress, double>(nameof(Percentage));

    public static readonly StyledProperty<IBrush> RingColorProperty =
        AvaloniaProperty.Register<RingProgress, IBrush>(nameof(RingColor), Brushes.CornflowerBlue);

    public static readonly StyledProperty<string> LabelProperty =
        AvaloniaProperty.Register<RingProgress, string>(nameof(Label), string.Empty);

    public double Percentage
    {
        get => GetValue(PercentageProperty);
        set => SetValue(PercentageProperty, value);
    }

    public IBrush RingColor
    {
        get => GetValue(RingColorProperty);
        set => SetValue(RingColorProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    private Path? _arcPath;

    static RingProgress()
    {
        PercentageProperty.Changed.AddClassHandler<RingProgress>((x, _) => x.UpdateArc());
    }

    public RingProgress()
    {
        InitializeComponent();
        _arcPath = this.FindControl<Path>("ArcPath");
        AttachedToVisualTree += (_, _) => UpdateArc();
    }

    private void UpdateArc()
    {
        if (_arcPath is null) return;

        const double size = 120;
        const double thickness = 10;
        var radius = (size - thickness) / 2;
        var center = new Point(size / 2, size / 2);

        var clamped = Math.Clamp(Percentage, 0.01, 99.999);
        var angle = clamped / 100.0 * 360.0;
        const double startAngle = -90.0;
        var endAngle = startAngle + angle;

        var startPoint = PointOnCircle(center, radius, startAngle);
        var endPoint = PointOnCircle(center, radius, endAngle);
        var isLargeArc = angle > 180;

        var geometry = new StreamGeometry();
        using (var ctx = geometry.Open())
        {
            ctx.BeginFigure(startPoint, false);
            ctx.ArcTo(endPoint, new Size(radius, radius), 0, isLargeArc, SweepDirection.Clockwise);
        }

        _arcPath.Data = geometry;
    }

    private static Point PointOnCircle(Point center, double radius, double angleDegrees)
    {
        var rad = angleDegrees * Math.PI / 180.0;
        return new Point(center.X + radius * Math.Cos(rad), center.Y + radius * Math.Sin(rad));
    }
}
