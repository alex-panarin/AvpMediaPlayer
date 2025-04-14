using AvpMediaPlayer.Data.Models;
using LiteDB;

namespace AvpMediaPlayer.Data
{
    public class MediaDataContext : DataContext, IMediaDataContext
    {
        public MediaDataContext(string connectionString, string dbname) 
            : base(connectionString, dbname)
        {
        }

        public void Rename(string listName, string newName)
        {
            ProcessCommand<MediaList>(listName, (list, lists) =>
            {
                if (list is null) return;

                list.Name = newName;
                lists.Update(list);
            });
        }

        public MediaList Add(string listName, string[] media)
        {
            return ProcessCommand<MediaList>(listName, (list, lists) =>
            {
                if (list is null)
                {
                    list = new MediaList
                    {
                        Name = listName,
                        Urls = media
                    };
                    lists.Insert(list);
                }
                else
                {
                    var urls = list.Urls
                        .Union(media)
                        .ToArray();

                    list.Urls = urls;
                    lists.Update(list);
                }
            });
        }

        public MediaList? Get(string listName)
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(_dbname);
            lists.EnsureIndex(x => x.Name);
            return lists.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();
        }

        public MediaList[] Get()
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(_dbname);
            return lists.Query().ToArray();
        }

        public void Delete(string listName)
        {
            ProcessCommand<MediaList>(listName, (list, lists) =>
            {
                if (list is null) return;

                lists.Delete(list.Id);
            });
        }

        public void Remove(string listName, string[] media)
        {
            ProcessCommand<MediaList>(listName, (list, lists) =>
            {
                if (list is null) return;

                var urls = list.Urls
                    .Except(media)
                    .ToArray();

                list.Urls = urls;
                lists.Update(list);
            });
        }

        public void Clear(string listName)
        {
            ProcessCommand<MediaList>(listName, (list, lists) =>
            {
                if (list is null) return;

                list.Urls = [];
                lists.Update(list);
            });
        }
    }
}
