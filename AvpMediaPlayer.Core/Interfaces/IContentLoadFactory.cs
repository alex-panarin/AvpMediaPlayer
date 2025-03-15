
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IContentLoadFactory
    {
        IAsyncEnumerable<ContentUIModel> Get();
        IAsyncEnumerable<ContentUIModel> Get(Content content);
    }
}
