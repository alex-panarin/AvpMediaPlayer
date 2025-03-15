using AvpMediaPlayer.UI.ViewModels;

namespace AvpMediaPlayer.App.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
        }

        public NavigationViewModel Navigation { get; } = new();
    }
}
