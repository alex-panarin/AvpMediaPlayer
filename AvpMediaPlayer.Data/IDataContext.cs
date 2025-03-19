using AvpMediaPlayer.Data.Models;

namespace AvpMediaPlayer.Data
{
    public interface IDataContext
    {
        void Add(string listName, string[] media);
        void Delete(string listName);
        MediaList[] Get();
        MediaList? Get(string listName);
        void Remove(string listName, string[] media);
        void Rename(string listName, string newName);
    }
}