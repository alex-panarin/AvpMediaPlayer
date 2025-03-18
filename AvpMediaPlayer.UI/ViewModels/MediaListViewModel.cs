using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class MediaListViewModel : ObservableObject
    {
        private readonly Action<ContentUIModel?> _onSelectedChanged;
        private readonly IContentUIFactory _contentFactory;
        private ContentUIModel? _SelectedItem;
        private MediaListModel? _SelectedList;
        private bool _IsPaneOpen;
        private bool _IsWaitLoad = false;

        public MediaListViewModel(Action<ContentUIModel?> onSelectedChanged,
            IContentUIFactory contentFactory)
        {
            _onSelectedChanged = onSelectedChanged;
            _contentFactory = contentFactory;

            PaneOpen = new(() => IsPaneOpen = !IsPaneOpen);
        }
        public ContentUIModel? SelectedItem
        { 
            get => _SelectedItem;
            internal set
            {
                SetProperty(ref _SelectedItem, value);
                _onSelectedChanged?.Invoke(value);
            }
        }
        public MediaListModel? SelectedList
        {
            get => _SelectedList;
            set
            {
                SetProperty(ref _SelectedList, value);
                using var locker = Items!.LockChangedEvent();
                Items.Clear();
                Items.AddRange(value is null ? [] : value.Contents);
            }
        }
        public LockedObservableCollection<ContentUIModel>? Items { get; } = [];
        public LockedObservableCollection<MediaListModel>? Lists { get; } = [];
        public RelayCommand PaneOpen { get; private set; }
        public bool IsPaneOpen
        {
            get => _IsPaneOpen;
            set => SetProperty(ref _IsPaneOpen, value);
        }
        public bool IsWaitLoad
        {
            get => _IsWaitLoad;
            set => SetProperty(ref _IsWaitLoad, value);
        }
        internal void AddMediaList(IReadOnlyList<IStorageItem> items)
        {
            IsWaitLoad = true;
            
            Task.Run(() =>
            {
                using var locker = Lists!.LockChangedEvent();
                var list = new MediaListModel();
                try
                {
                    var contentpaths = items.Select(i => i.TryGetLocalPath());
                    foreach (var content in _contentFactory.Get(contentpaths!))
                    {
                        if (content.IsDirectory)
                        {
                            list.Title ??= content.Title;
                            list.Contents.AddRange(_contentFactory.Get(content.Model!));
                        }
                        else
                        {
                            list.Title ??= content.Model?.ParentName;
                            list.Contents.Add(content);
                        }
                    }
                }
                finally
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        if (list.Contents.Any())
                        {
                            IsPaneOpen = true;
                            Lists.Add(list);
                            SelectedList = list;
                        }
                        IsWaitLoad = false;
                    });
                }
            });
        }
    }
}
