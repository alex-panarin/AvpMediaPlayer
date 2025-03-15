using Avalonia.Controls;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.Media.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class MediaContainerViewModel : ObservableObject
    {
        private readonly Action<ContentUIModel?> _onSelectedChanged;
        private ContentUIModel? _SelectedItem;

        public MediaContainerViewModel(Action<ContentUIModel?> onSelectedChanged)
        {
            //if(Design.IsDesignMode)
            {
                Items =
                [
                    new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Artcore.flac"))),
                    new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Beyond the Senses.flac"))),
                    new ContentUIModel(new AudioMediaContent(new FileContent(@"E:\MUSIC\Astrix\Astrix - Astrix The Best of - Monster(remix).flac"))),
                ];
            }

            _onSelectedChanged = onSelectedChanged;
            //else
            //    Items = [];
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
    }
}
