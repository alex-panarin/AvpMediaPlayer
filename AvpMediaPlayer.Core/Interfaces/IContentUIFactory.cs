
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IContentUIFactory
    {
        IEnumerable<ContentUIModel> Get(Content content);
        IEnumerable<ContentUIModel> Get(IEnumerable<string> paths);
    }
}
