using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        private ContentUIModel? _SelectedItem;

        public NavigationViewModel()
        {
            Ribbon = new(OnButtonClick);
            Container = new();
            CloseApp = new(() => 
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    desktop.Shutdown();
            });
        }

        public RibbonViewModel Ribbon { get; }
        public MediaContainerViewModel Container { get; }
        public ContentUIModel? SelectedItem 
        { 
            get => _SelectedItem;
            set
            {
                SetProperty(ref _SelectedItem, value);
                Ribbon.SelectedItem = _SelectedItem;
                Container.SelectedItem = _SelectedItem;
            } 
        }
        public RelayCommand CloseApp { get; }
        private void OnButtonClick(RibbonModel? model)
        {
            switch (model?.Action)
            {
                case RibbonModel.List:
                    break;
                case RibbonModel.Stop:
                    break;
                case RibbonModel.Play:
                    break;
                case RibbonModel.Pause:
                    break;
                case RibbonModel.Show:
                    break;
                default:
                    break;
            }
        }

    }
}
