using AvpMediaPlayer.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.Models
{
    public class MediaListModel : ObservableObject
    {
        private string? _Title;
        private bool _IsNeedRename;

        public MediaListModel()
        {
        }
        public string? Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        public LockedObservableCollection<ContentUIModel> Contents { get; internal set; } = [];
        public RelayCommand<string>? ListCommand { get; set; }
        public bool IsNeedRename
        {
            get => _IsNeedRename;
            set => SetProperty(ref _IsNeedRename, value);
        }
    }
}
