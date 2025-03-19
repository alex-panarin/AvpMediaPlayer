using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Selection;
using Avalonia.Platform.Storage;
using AvpMediaPlayer.Core;
using AvpMediaPlayer.Core.Helpers;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.Media.Models;
using AvpMediaPlayer.UI.Models;
using AvpMediaPlayer.UI.Repositories;
using AvpMediaPlayer.UI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        private ContentUIModel? _SelectedItem;
        private MediaListWindow? _listWindow;
        private string? _SelectedText;
        private readonly FilePickerFileType _filter;
        const string _NewListName = "Новый список";
        public NavigationViewModel(FilePickerFileType filter)
        {
            var patterns = filter.Patterns?.Select(p => p.Replace("*",""));
            Ribbon = new(async (m) => await OnButtonClick(m));
            MediaList = new(OnSelectedChanged
                , new MediaListRepository(new ContentUIFactory(new LocalContentRepository(new LocalContentProvider())
                , new MediaContentFactory()
                , (c) => patterns?.Any(f => c.Url.Contains(f)) == true)));
          
            _filter = filter;
        }
        public string? SelectedText 
        { 
            get => _SelectedText; 
            set => SetProperty(ref _SelectedText, value); 
        }
        public RibbonViewModel Ribbon { get; }
        public MediaListViewModel MediaList { get; }
        public ContentUIModel? SelectedItem 
        { 
            get => _SelectedItem;
            set
            {
                SetProperty(ref _SelectedItem, value);
                Ribbon.SelectedItem = _SelectedItem;
                SelectedText = _SelectedItem?.Title;
            } 
        }
        public RelayCommand? CloseApp { get; set; }
        private async Task OnButtonClick(RibbonModel? model)
        {
            _listWindow ??= new MediaListWindow() { DataContext = MediaList };

            switch (model?.Action)
            {
                case RibbonModel.List:
                    _listWindow.IsVisible = !_listWindow.IsVisible;
                    break;
                case RibbonModel.Stop:
                case RibbonModel.Play:
                case RibbonModel.Pause:
                    await ProcessMediaCommand(model);
                    break;
                case RibbonModel.Next:
                case RibbonModel.Prev:
                    ProcessNavigationCommand(model);
                    break;
                case RibbonModel.Show:
                    break;
                case RibbonModel.AddTrack:
                case RibbonModel.AddList:
                case RibbonModel.NewList:
                    await ProcessMediaList(model);
                    break;
                default:
                    break;
            }
        }
        private void ProcessNavigationCommand(RibbonModel model)
        {
            if (SelectedItem is null 
                || MediaList.Items is null) return;

            int index = MediaList.Items.IndexOf(SelectedItem);
            var ni = model.Action == RibbonModel.Next ? index + 1 : index - 1;
            if (ni < 0) ni = MediaList.Items.Count - 1;
            if (ni >= MediaList.Items.Count) ni = 0;

            MediaList.SelectedItem = MediaList.Items[ni];
        }
        private async Task ProcessMediaList(RibbonModel model)
        {
            var topLevel = TopLevel.GetTopLevel(_listWindow);
            var provider = topLevel?.StorageProvider;

            if (provider is null) return;
            
            var startLocation = await provider.TryGetFolderFromPathAsync(@"E:\Music"); // TODO: from Settings
            IReadOnlyList<IStorageItem>? items = null;
            if (model.Action == RibbonModel.AddList)
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
            else if(model.Action == RibbonModel.NewList)
            {
                var list = MediaList?.Lists?.FirstOrDefault(x => x.Title == _NewListName);
                if(list == null)
                {
                    list = new MediaListModel
                    {
                        Title = _NewListName
                    };
                    MediaList?.Lists?.Add(list);
                }

                MediaList!.SelectedList = list;
            }
            if (items!.IsEmpty() == true) return;

            MediaList.AddMediaList(items!);
        }
        private async Task ProcessMediaCommand(RibbonModel model)
        {
            await Task.CompletedTask;
        }
        private void OnSelectedChanged(ContentUIModel? item)
            => SelectedItem = item;
    }
}
