using AvpMediaPlayer.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvpMediaPlayer.UI.Models
{
    public class MediaListModel : ObservableObject
    {
        private string? _Title;
        public string? Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        public LockedObservableCollection<ContentUIModel> Contents { get; } = [];
    }
}
