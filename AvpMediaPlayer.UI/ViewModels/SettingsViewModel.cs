using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Runtime;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsProvider? _settingsProvider;
        private SettingsModel? _Settings;

        public SettingsViewModel()
        { }
        public SettingsViewModel(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider;
            Settings = _settingsProvider?.Get();
        }

        public SettingsModel?  Settings
        { 
            get => _Settings; 
            set => SetProperty(ref _Settings, value); 
        }
    }
}
