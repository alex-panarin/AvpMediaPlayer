using LiteDB;

namespace AvpMediaPlayer.Data.Models
{
    public class MediaList : Entity
    {
        public string[] Urls { get; set; } = [];
    }
}
