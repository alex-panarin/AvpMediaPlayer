using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Media.Models
{
    internal class ContainerMediaContent : IMediaContent
    {
        public ContainerMediaContent(Content content)
        {
            Content = content;
        }
        public IMediaTag? Tag { get; } = null;

        public Content Content { get; }

        public string? Description => Content?.Name;
    }
}