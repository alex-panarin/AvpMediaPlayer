using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using AvpMediaPlayer.App.Models;
using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Media.Interfaces;
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
        public MainWindowViewModel() { }

        public MainWindowViewModel(ISettingsProvider settingsProvider
            , IWindowRepository repository)
        {
            Navigation = new(_filter, settingsProvider)
            {
                CloseApp = new(() =>
                {
                    if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                        desktop.Shutdown();
                })
            };

            MediaManagement = Navigation.MediaManagement;
            Repository = repository;
        }

        public NavigationViewModel? Navigation { get; }

        public IMediaManagement? MediaManagement { get; }
        public IWindowRepository? Repository { get; }
    }
}
