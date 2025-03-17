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
        private MediaListWindow? _listWindow;

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
        public MediaListViewModel Container { get; }
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
                    _listWindow ??= new MediaListWindow() { DataContext = Container };
                    _listWindow.IsVisible = !_listWindow.IsVisible;
                    break;
                case RibbonModel.Stop:
                case RibbonModel.Play:
                case RibbonModel.Pause:
                case RibbonModel.Next:
                case RibbonModel.Prev:
                    ProcessMediaCommand(model);
                    break;
                case RibbonModel.Show:
                    break;
                case RibbonModel.AddTrack:
                case RibbonModel.AddList:
                case RibbonModel.Open:
                    ProcessMediaList(model);
                    break;
                default:
                    break;
            }
        }
        private void ProcessMediaList(RibbonModel model)
        {
            
        }
        private void ProcessMediaCommand(RibbonModel model)
        {
            
        }
        private void OnSelectedChanged(ContentUIModel? item)
            => SelectedItem = item;
    }
}
