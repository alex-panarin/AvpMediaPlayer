using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace AvpMediaPlayer.UI.Views;

public partial class VisualizationWindow : Window
{
    private readonly Window? _mainWindow;

    public VisualizationWindow()
    {
        InitializeComponent();

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            _mainWindow ??= desktop.MainWindow;

        if (_mainWindow is not null)
        {
            _mainWindow.PositionChanged += _mainWindow_PositionChanged;
        }
    }
    private void _mainWindow_PositionChanged(object? sender, PixelPointEventArgs e)
    {
        PositionWindow();
    }
    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        PositionWindow();
        base.OnSizeChanged(e);
    }
    private void PositionWindow()
    {
        if (_mainWindow is not null)
        {
            this.Position = _mainWindow.PointToScreen(_mainWindow.Bounds.TopRight);
        }
    }
}