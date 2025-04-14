using AvpMediaPlayer.Data.Models;
using LiteDB;

namespace AvpMediaPlayer.Data
{
    public abstract class DataContext
    {
        protected readonly string _connectionString;
        protected readonly string _dbname;
        public DataContext(string connectionString, string dbname)
        {
            _connectionString = connectionString;
            _dbname = dbname;
        }

        protected TEntity ProcessCommand<TEntity>(string listName, Action<TEntity?, ILiteCollection<TEntity>> action)
            where TEntity : Entity
        {
            using var db = new LiteDatabase(_connectionString);
            
            var entities = db.GetCollection<TEntity>(_dbname);
            entities.EnsureIndex(x => x.Name);

            var entity = entities.Query()
                .Where(x => x.Name == listName)
                .FirstOrDefault();

            action(entity, entities);

            return entity;
        }
    }
}
