using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IContentProvider
    {
        Content GetContent(string url);
        IEnumerable<Content> GetContents(string parent);
    }
}
