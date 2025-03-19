using AvpMediaPlayer.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvpMediaPlayer.UI.Models
{
    public class MediaListModel : ObservableObject
    {
        public MediaListModel()
        {
            ListCommand = new((cmd) => OnListCommand(cmd));
        }
        private string? _Title;
        public string? Title
        {
            get => _Title;
            set => SetProperty(ref _Title, value);
        }
        public LockedObservableCollection<ContentUIModel> Contents { get; internal set; } = [];
        public RelayCommand<string> ListCommand { get; private set; }
        private void OnListCommand(string? cmd)
        {

        }
    }
}
