using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IMediaContentFactory
    {
        IMediaContent Create(Content content);
    }
}
