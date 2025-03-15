using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using AvpMediaPlayer.UI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        private ContentUIModel? _SelectedItem;
        private MediaContainerWindow? _listWindow;

        public NavigationViewModel()
        {
            Ribbon = new(OnButtonClick);
            Container = new(OnSelectedChanged);
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
            } 
        }
        public RelayCommand CloseApp { get; }
        private void OnButtonClick(RibbonModel? model)
        {
            switch (model?.Action)
            {
                case RibbonModel.List:
                    _listWindow ??= new MediaContainerWindow() { DataContext = Container };
                    _listWindow.IsVisible = !_listWindow.IsVisible;
                    break;
                case RibbonModel.Stop:
                    break;
                case RibbonModel.Play:
                    break;
                case RibbonModel.Pause:
                    break;
                case RibbonModel.Show:
                    break;
                case RibbonModel.AddFiles:
                    break;
                case RibbonModel.AddFolder:
                    break;
                case RibbonModel.Open:
                    break;
                case RibbonModel.Next:
                    break;
                case RibbonModel.Prev:
                    break;
                default:
                    break;
            }
        }
        private void OnSelectedChanged(ContentUIModel? item)
            => SelectedItem = item;
    }
}
