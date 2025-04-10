using Avalonia.Platform.Storage;
using Avalonia.Threading;
using AvpMediaPlayer.Core.Helpers;
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
        const string RenameBeginCmd = "rename_begin";
        const string RenameEndCmd = "rename_end";
        const string DeleteCmd = "delete";
        const string ClearCmd = "clear";
        public MediaListViewModel(Action<ContentUIModel?> onSelectedChanged,
            IMediaListRepository mediaListRepository)
        {
            _onSelectedChanged = onSelectedChanged;
            _mediaListRepository = mediaListRepository;

            PaneOpen = new(() => IsPaneOpen = !IsPaneOpen);
        }

        public Action? DoubleClick { get; set; }

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
                var list = SelectedList;
                try
                {
                    var contentpaths = items.Select(i => i.TryGetLocalPath()!);
                    list = _mediaListRepository.AddOrUpdate(list!, [.. contentpaths]);
                }
                finally
                {
                    Dispatcher.UIThread.Invoke(() =>
                    {
                        if (list?.Contents.Any() == true)
                        {
                            list.ListCommand = new((c) => OnListCommand(list, c));

                            IsPaneOpen = true;
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
                            lists.ForEach(l => l.ListCommand = new((c) => OnListCommand(l, c)));

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

        internal void OnListCommand(MediaListModel list, string? cmd)
        {
            if(cmd == RenameBeginCmd)
            {
                _OldTitle = list.Title;
                list.IsNeedRename = true;
            }
            else if(cmd == DeleteCmd)
            {
                _mediaListRepository.Delete(list);
                Lists?.Remove(list);
                list.ListCommand = null; // Освобождаем ссылку
            }
            else if (cmd == RenameEndCmd)
            {
                list.IsNeedRename = false;
                _mediaListRepository.Rename(_OldTitle, list);
                _OldTitle = null;
            }
            else if(cmd == ClearCmd)
            {
                if (Items is not null)
                {
                    using (Items.LockChangedEvent())
                        Items.Clear();
                }

                using (list.Contents.LockChangedEvent())
                {
                    _mediaListRepository.Clear(list);
                    list.Contents.Clear();
                }
            }
        }

        private string? _OldTitle = null;

        internal void OnDoubleClick()
        {
            DoubleClick?.Invoke();
        }
    }
}
