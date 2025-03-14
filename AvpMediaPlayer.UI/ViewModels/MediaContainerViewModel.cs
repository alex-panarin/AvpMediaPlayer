using AvpMediaPlayer.Core.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class MediaContainerViewModel : ObservableObject
    {
        public MediaContainerViewModel()
        {
        }

        public ContentUIModel? SelectedItem { get; internal set; }
    }
}
