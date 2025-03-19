using AvpMediaPlayer.UI.Models;

namespace AvpMediaPlayer.UI.Repositories
{
    public interface IMediaListRepository
    {
        MediaListModel[] Get();
        void Add(MediaListModel listModel);
        MediaListModel New(string[] urls);
    }
}
