using AvpMediaPlayer.UI.Models;

namespace AvpMediaPlayer.UI.Repositories
{
    public interface IMediaListRepository
    {
        MediaListModel[] Get();
        void Add(MediaListModel listModel);
        MediaListModel AddOrUpdate(MediaListModel listModel, string[] urls);
        void Rename(string? oldName, MediaListModel listModel);
        void Delete(MediaListModel list);
    }
}
