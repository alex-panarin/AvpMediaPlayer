using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.Media.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class MediaContainerViewModel : ObservableObject
    {
        private readonly Action<ContentUIModel?> _onSelectedChanged;
        private ContentUIModel? _SelectedItem;
        private bool _IsPaneOpen;

        public MediaContainerViewModel(Action<ContentUIModel?> onSelectedChanged)
        {
            Items =
            [
                new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Artcore.flac"))),
                new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Beyond the Senses.flac"))),
                new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Monster(remix).flac"))),
            ];
            
            _onSelectedChanged = onSelectedChanged;

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

        public ObservableCollection<ContentUIModel> Items { get; }

        public RelayCommand PaneOpen { get; private set; }

        private bool IsPaneOpen
        {
            get => _IsPaneOpen;
            set => SetProperty(ref _IsPaneOpen, value);
        }
    }
}
