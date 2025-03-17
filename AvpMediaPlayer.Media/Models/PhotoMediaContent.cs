using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Media.Models
{
    internal class PhotoMediaContent : AudioMediaContent
    {
        public PhotoMediaContent(Content content, IMediaTag? mediaTag = null)
            : base(content, mediaTag)
        {
        }
    }
}