using Avalonia.Platform.Storage;
using AvpMediaPlayer.UI.ViewModels;

namespace AvpMediaPlayer.App.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        static FilePickerFileType _filter = new("Audio")
        { 
            Patterns = //["*.mp3", "*.ogg", "*.flac", "*.wav", "*.jpg", "*.jpeg", "*.bmp", "*.tiff", "*.png"] 
                ["*.mp3", "*.ogg", "*.flac", "*.wav", "*.wma"]
        };
        public MainWindowViewModel()
        {
            Navigation = new(_filter);
        }

        public NavigationViewModel Navigation { get; }
    }
}
