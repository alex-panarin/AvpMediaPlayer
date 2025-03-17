using Avalonia.Platform.Storage;
using AvpMediaPlayer.UI.ViewModels;
using System.Linq;

namespace AvpMediaPlayer.App.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        static FilePickerFileType _filter = new("Audio")
        { 
            Patterns = ["*.mp3", "*.ogg", "*.flac", "*.wav", "*.jpg", "*.jpeg", "*.bmp", "*.tiff", "*.png"] 
        };
        public MainWindowViewModel()
        {
            Navigation = new(_filter);
        }

        public NavigationViewModel Navigation { get; }
    }
}
