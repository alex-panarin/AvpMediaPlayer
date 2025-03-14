using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IMediaContentProvider
    {
        IMediaContent? GetContent(Content content);
    }
}
