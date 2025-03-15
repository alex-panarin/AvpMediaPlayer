using Avalonia.Controls;

namespace AvpMediaPlayer.UI.Controls;

public partial class RibbonControl : UserControl
{
    public RibbonControl()
    {
        InitializeComponent();
    }
    protected override void OnDataContextEndUpdate()
    {
        base.OnDataContextEndUpdate();
    }
}