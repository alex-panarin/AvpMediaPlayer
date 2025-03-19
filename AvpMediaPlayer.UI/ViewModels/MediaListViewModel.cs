using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using AvpMediaPlayer.UI.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class MediaListViewModel : ObservableObject
    {
        private readonly Action<ContentUIModel?> _onSelectedChanged;
        private readonly IMediaListRepository _mediaListRepository;
        private ContentUIModel? _SelectedItem;
        private MediaListModel? _SelectedList;
        private bool _IsPaneOpen;
        private bool _IsWaitLoad = false;

        public MediaListViewModel(Action<ContentUIModel?> onSelectedChanged,
            IMediaListRepository mediaListRepository)
        {
            _onSelectedChanged = onSelectedChanged;
            _mediaListRepository = mediaListRepository;

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
        public LockedObservableCollection<MediaListModel>? Lists { get; private set; } = [];
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
                MediaListModel? list = null;
                try
                {
                    var contentpaths = items.Select(i => i.TryGetLocalPath()!);
                    list = _mediaListRepository.New([.. contentpaths]);
                }
                finally
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        if (list?.Contents.Any() == true)
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
        internal void LoadMediaLists()
        {
            IsWaitLoad = true;
            Task.Run(() =>
            {
                using var locker = Lists!.LockChangedEvent();
                MediaListModel[]? lists = null;
                try
                {
                    lists = _mediaListRepository.Get();
                }
                finally
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        if (lists is not null)
                        {
                            Lists.Clear();
                            Lists.AddRange(lists);
                            if(Lists.Any())
                            {
                                SelectedList = Lists[0];
                            }
                        }
                        IsWaitLoad = false;
                    });
                }
            });
        }
    }
}
