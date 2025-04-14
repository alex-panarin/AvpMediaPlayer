using Avalonia.Controls;

namespace AvpMediaPlayer.App.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    protected override void OnSizeChanged(SizeChangedEventArgs e)
    {
        base.OnSizeChanged(e);
        var screen = this.Screens.ScreenFromVisual(this);
    }
}