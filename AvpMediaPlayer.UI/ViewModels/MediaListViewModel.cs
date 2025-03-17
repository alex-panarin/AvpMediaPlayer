using Avalonia.Platform.Storage;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.Media.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class MediaListViewModel : ObservableObject
    {
        private readonly Action<ContentUIModel?> _onSelectedChanged;
        private readonly IMediaContentFactory _contentFactory;
        private ContentUIModel? _SelectedItem;
        private bool _IsPaneOpen;
        private bool _IsWaitLoad = false;
        private ObservableCollection<ContentUIModel>? _Items;

        public MediaListViewModel(Action<ContentUIModel?> onSelectedChanged,
            IMediaContentFactory contentFactory)
        {
            Items =
            [
                new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Artcore.flac"))),
                new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Beyond the Senses.flac"))),
                new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Monster(remix).flac"))),
            ];
            
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
        public ObservableCollection<ContentUIModel>? Items 
        { 
            get => _Items;
            private set => SetProperty(ref _Items, value);
        }
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
            var contents = items.Select(i =>
            {
                return new ContentUIModel(_contentFactory.Create(i is IStorageFolder folder
                    ? new DirectoryContent(i.TryGetLocalPath()!)
                    : new FileContent(i.TryGetLocalPath()!)));
            });
            Items = [.. contents];
        }
    }
}
