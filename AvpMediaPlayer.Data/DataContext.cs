using AvpMediaPlayer.Data.Models;
using LiteDB;

namespace AvpMediaPlayer.Data
{
    public class DataContext : IDataContext
    {
        private readonly string _connectionString;
        const string dbname = "medialist";
        public DataContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void Rename(string listName, string newName)
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(dbname);

            var list = lists.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();

            if (list is null) return;

            list.Name = newName;
            lists.Update(list);
        }
        public void Add(string listName, string[] media)
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(dbname);

            var list = lists.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();

            if (list is null)
            {
                lists.Insert(new MediaList
                {
                    Name = listName,
                    Urls = media
                });
                return;
            }

            var urls = list.Urls
                .Union(media)
                .ToArray();

            list.Urls = urls;
            lists.Update(list);
        }
        public MediaList? Get(string listName)
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(dbname);
            lists.EnsureIndex(x => x.Name);

            return lists.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();
        }
        public MediaList[] Get()
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(dbname);
            return lists.Query().ToArray();
        }
        public void Delete(string listName)
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(dbname);
            lists.EnsureIndex(x => x.Name);

            var list = lists.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();

            if (list is null) return;

            lists.Delete(list.Id);
        }
        public void Remove(string listName, string[] media)
        {
            using var db = new LiteDatabase(_connectionString);
            var lists = db.GetCollection<MediaList>(dbname);
            lists.EnsureIndex(x => x.Name);

            var list = lists.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();

            if (list is null) return;

            var urls = list.Urls
                .Except(media)
                .ToArray();
            
            list.Urls = urls;
            lists.Update(list);
        }
    }
}
