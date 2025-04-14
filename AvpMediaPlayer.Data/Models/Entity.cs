using LiteDB;

namespace AvpMediaPlayer.Data.Models
{
    public class Entity
    {
        [BsonId(true)]
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
