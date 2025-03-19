using LiteDB;

namespace AvpMediaPlayer.Data.Models
{
    public class MediaList
    {
        [BsonId(true)]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string[] Urls { get; set; } = [];
    }
}
