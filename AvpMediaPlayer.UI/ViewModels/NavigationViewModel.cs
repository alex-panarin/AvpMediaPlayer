﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Selection;
using Avalonia.Platform.Storage;
using AvpMediaPlayer.Core;
using AvpMediaPlayer.Core.Helpers;
using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.Data.Models;
using AvpMediaPlayer.Media.Audio;
using AvpMediaPlayer.Media.Interfaces;
using AvpMediaPlayer.Media.Models;
using AvpMediaPlayer.UI.Models;
using AvpMediaPlayer.UI.Repositories;
using AvpMediaPlayer.UI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class NavigationViewModel 
        : ObservableObject
        , INavigation
    {
        private ContentUIModel? _SelectedItem;
        private MediaListWindow? _listWindow;
        private VisualizationWindow? _visualWindow;
        private IMediaPlayer _mediaPlayer;
        private readonly SettingsViewModel _settings;
        private IMediaManagement _mediaManagement;
        private IVisualizer _visualizer;
        private string? _SelectedText;
        private SettingsWindow? _settingsWindow;
        private readonly FilePickerFileType _filter;
        const string _NewListName = "Новый список";
        const string _StaticTitle = "AvpMediaPlayer v1.0";

        public NavigationViewModel(FilePickerFileType filter
            , ISettingsProvider settingsProvider)
        {
            var patterns = filter.Patterns?.Select(p => p.Replace("*",""));
            Ribbon = new(async (m) => await OnButtonClick(m));
            MediaList = new(OnSelectedChanged
                , new MediaListRepository(new ContentUIFactory(new LocalContentRepository(new LocalContentProvider())
                , new MediaContentFactory()
                , (c) => patterns?.Any(f => c.Url.Contains(f)) == true)));
          
            _filter = filter;
            _mediaManagement = new AudioMediaManagement();
            _visualizer = new AudioVisualizer();
            _mediaPlayer = new AudioPlayer(_mediaManagement, _visualizer, this);
            _settings = new SettingsViewModel(settingsProvider);
            var settings = _settings.Settings;

            if (settings is not null)
            {
                _mediaManagement.LoopTrack = settings.LoopTrack;
                _mediaManagement.LoopCatalog = settings.LoopCatalog;
                _mediaManagement.LoopLists = settings.LoopLists;
            }
            
            MediaList.Settings = settings;
            MediaList.DoubleClick = () =>
            {
                if (_SelectedItem is not null)
                {
                    _mediaPlayer.MediaContent = _SelectedItem.MediaContent;
                    _mediaPlayer.Play();
                }
            };
        }

        public string? SelectedText 
        { 
            get => _SelectedText ?? _StaticTitle; 
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
                if(_SelectedItem is not null)
                    _mediaPlayer.MediaContent = _SelectedItem.MediaContent;
            } 
        }

        public RelayCommand? CloseApp { get; set; }

        public IMediaManagement MediaManagement => _mediaManagement;

        private async Task OnButtonClick(RibbonModel? model)
        {
            _listWindow ??= new MediaListWindow() { DataContext = MediaList };
            _visualWindow ??= new VisualizationWindow() { DataContext = _visualizer };
            _settingsWindow ??= new SettingsWindow() { DataContext = _settings };

            switch (model?.Action)
            {
                case RibbonModel.List:
                    _listWindow.IsVisible = !_listWindow.IsVisible;
                    break;
                case RibbonModel.Sets:
                    _settingsWindow.IsVisible = !_settingsWindow.IsVisible;
                    break;
                case RibbonModel.Show:
                    _visualWindow.IsVisible = !_visualWindow.IsVisible;
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
            var ni = model.Action == RibbonModel.Next ? 1 : - 1;
            ((INavigation)this).NavigateNextItem(ni);
        }

        void INavigation.NavigateNextItem(int offset)
        {
            if (SelectedItem is null
                || MediaList.Items is null) return;

            int index = MediaList.Items.IndexOf(SelectedItem) + offset;
            if (index < 0) index = MediaList.Items.Count - 1;
            if (index >= MediaList.Items.Count) index = 0;

            SelectedItem = MediaList.SelectedItem = MediaList.Items[index];

            _mediaPlayer.Play();
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
                var list = MediaList.Lists?.FirstOrDefault(x => x.Title == _NewListName);
                if(list == null)
                {
                    list = new MediaListModel
                    {
                        Title = _NewListName,
                    };
                    list.ListCommand = new((c) => MediaList.OnListCommand(list, c));
                    MediaList.Lists?.Add(list);
                }
                
                MediaList.IsPaneOpen = true; 

                MediaList.SelectedList = list;
            }
            
            if (items!.IsEmpty() == true) return;

            MediaList.AddMediaList(items!);
        }

        private async Task ProcessMediaCommand(RibbonModel model)
        {
            switch(model.Action)
            {
                case RibbonModel.Stop:
                    _mediaPlayer.Stop();
                    break;
                case RibbonModel.Play:
                    _mediaPlayer.Play();
                    break;
                case RibbonModel.Pause:
                    _mediaPlayer.Pause();
                    break;
            }
            await Task.CompletedTask;
        }

        private void OnSelectedChanged(ContentUIModel? item)
            => SelectedItem = item;

        public void SaveSettings()
        {
            var list = MediaList?.SelectedList?.Title ?? string.Empty; 
            var track = MediaList?.SelectedItem?.Title ?? string.Empty;
            _settings!.Settings!.LastList = list;
            _settings!.Settings!.LastTrack = track;
            _settings.SaveSettings();
        }
    }
}
