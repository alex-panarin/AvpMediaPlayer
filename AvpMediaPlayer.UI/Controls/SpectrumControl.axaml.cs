using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvpMediaPlayer.Core.Helpers;

namespace AvpMediaPlayer.UI.Controls;

public partial class SpectrumControl : UserControl
{
    public readonly static DirectProperty<SpectrumControl, double[]?> SpectrumProperty =
        AvaloniaProperty.RegisterDirect<SpectrumControl, double[]?>(nameof(Spectrum), (c) => c._Spectrum);

    private readonly Pen _linePen = new(new SolidColorBrush(Colors.LimeGreen), 0.8d);
    private double[]? _Spectrum;

    public SpectrumControl()
    {
        InitializeComponent();
    }

    public double[]? Spectrum 
    { 
        get => _Spectrum; 
        set
        {
            SetAndRaise(SpectrumProperty, ref _Spectrum, value);
            InvalidateVisual();
        } 
    }
    public override void Render(DrawingContext context)
    {
        base.Render(context);
        DrawLinearStyle(context, container.Bounds, Spectrum);
    }
    private void DrawLinearStyle(DrawingContext context, Rect clipRect, double[]? data)
    {
        if(data == null) return;

        var pointWidth = clipRect.Width / data.Length;
        var height = clipRect.Height;
        Point ps = new Point(clipRect.X, clipRect.Top + height - data[0]);

        data.ForEach((value, index) =>
        {
            if (index > 0)
            {
                var pe = new Point(clipRect.X + (index * pointWidth), clipRect.Top + height - value);
                context.DrawLine(_linePen, ps, pe);
                ps = pe;
            }
        });
    }
}