using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IMediaTagProvider
    {
        IMediaTag? GetTag(Content content);
    }
}
