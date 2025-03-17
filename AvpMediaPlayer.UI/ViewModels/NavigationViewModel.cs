using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using AvpMediaPlayer.Core.Helpers;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.Media.Models;
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
        private readonly FilePickerFileType _filter;

        public NavigationViewModel(FilePickerFileType filter)
        {
            Ribbon = new(async (m) => await OnButtonClick(m));
            Container = new(OnSelectedChanged, new MediaContentFactory());
            CloseApp = new(() => 
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                    desktop.Shutdown();
            });
            _filter = filter;
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
        private async Task OnButtonClick(RibbonModel? model)
        {
            _listWindow ??= new MediaListWindow() { DataContext = Container };

            switch (model?.Action)
            {
                case RibbonModel.List:
                    _listWindow.IsVisible = !_listWindow.IsVisible;
                    break;
                case RibbonModel.Stop:
                case RibbonModel.Play:
                case RibbonModel.Pause:
                case RibbonModel.Next:
                case RibbonModel.Prev:
                    await ProcessMediaCommand(model);
                    break;
                case RibbonModel.Show:
                    break;
                case RibbonModel.AddTrack:
                case RibbonModel.AddList:
                case RibbonModel.Open:
                    await ProcessMediaList(model);
                    break;
                default:
                    break;
            }
        }
        private async Task ProcessMediaList(RibbonModel model)
        {
            var topLevel = TopLevel.GetTopLevel(_listWindow);
            var provider = topLevel?.StorageProvider;

            if (provider is null) return;
            
            var startLocation = await provider.TryGetFolderFromPathAsync(@"E:\Music"); // TODO: from Settings
            IReadOnlyList<IStorageItem>? items = null;
            if (model.Action == RibbonModel.Open
                || model.Action == RibbonModel.AddList)
            {
                items = await provider.OpenFolderPickerAsync(new FolderPickerOpenOptions
                {
                    Title = "Open Folders",
                    AllowMultiple = model.Action == RibbonModel.AddList,
                    SuggestedStartLocation = startLocation
                });
                
            }
            else if (model.Action == RibbonModel.AddTrack)
            {
                items = await provider.OpenFilePickerAsync(new FilePickerOpenOptions
                {
                    FileTypeFilter = [_filter],
                    Title = "Add Files",
                    AllowMultiple = true,
                    SuggestedStartLocation = startLocation
                });
            }
            
            if (items?.IsEmpty() == true) return;

            Container.AddMediaList(items!);
        }
        private Task ProcessMediaCommand(RibbonModel model)
        {
            return Task.CompletedTask;
        }
        private void OnSelectedChanged(ContentUIModel? item)
            => SelectedItem = item;
    }
}
