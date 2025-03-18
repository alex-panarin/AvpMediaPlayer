using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IContentRepository
    {
        IEnumerable<Content> Get(Content parent);
        Content Get(string path);
    }
}
