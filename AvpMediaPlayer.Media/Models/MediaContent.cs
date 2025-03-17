using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Media.Models
{
    public abstract class MediaContent : IMediaContent
    {
        public MediaContent(Content content, IMediaTag? mediaTag = null)
        {
            Content = content;
            Tag = mediaTag;
        }
        public Content? Content { get; }
        public abstract string? Description { get; }
        public bool HasContent => Content?.IsDirectory == false;
        public virtual IMediaTag? Tag { get; protected set; }
    }
}
