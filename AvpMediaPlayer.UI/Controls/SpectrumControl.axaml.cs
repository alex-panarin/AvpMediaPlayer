using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvpMediaPlayer.Core.Helpers;


namespace AvpMediaPlayer.UI.Controls;

public partial class SpectrumControl : UserControl
{
    public readonly static DirectProperty<SpectrumControl, double[]?> SpectrumProperty =
        AvaloniaProperty.RegisterDirect<SpectrumControl, double[]?>(nameof(Spectrum), (c) => c.Spectrum, (c, v) => c.Spectrum = v);
    public readonly static DirectProperty<SpectrumControl, double[]?> LevelsProperty =
        AvaloniaProperty.RegisterDirect<SpectrumControl, double[]?>(nameof(Levels), (c) => c.Levels, (c, v) => c.Levels = v);

    private readonly Pen _linePen = new(new SolidColorBrush(Colors.LimeGreen), 0.8d);
  
    private double[]? _Spectrum;
    private double[]? _Levels;
  
    public SpectrumControl()
    {
        InitializeComponent();
    }

    public double[]? Levels
    {
        get => _Levels;
        set
        {
            SetAndRaise(LevelsProperty, ref _Levels, value);
            
            if (_Levels != null)
            {
                var height = container.Bounds.Height;
                levelLeft.Height = _Levels[0] * height;
                levelRight.Height = _Levels[1] * height;
            }
        }
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

    protected override void OnDataContextEndUpdate()
    {
        base.OnDataContextEndUpdate();
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