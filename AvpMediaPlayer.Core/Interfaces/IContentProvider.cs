using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IContentProvider
    {
        IAsyncEnumerable<Content> GetContents(Content content);
        IAsyncEnumerable<Content> GetContents(string root);
    }

    
}
