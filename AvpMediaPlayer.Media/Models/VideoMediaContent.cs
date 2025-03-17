using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Media.Models
{
    internal class VideoMediaContent : AudioMediaContent
    {
        public VideoMediaContent(Content content, IMediaTag? mediaTag = null) 
            : base(content, mediaTag)
        {
        }
    }
}