using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IMediaContent
    {
        IMediaTag? MediaTag { get; }
        Content? Content { get; }
        string? Description { get; }
    }
}
