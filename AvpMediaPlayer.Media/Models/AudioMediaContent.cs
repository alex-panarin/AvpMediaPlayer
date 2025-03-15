using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Media.Models
{
    public class AudioMediaContent : MediaContent
    {
        public AudioMediaContent(Content content, IMediaTag? mediaTag = null) 
            : base(content, mediaTag)
        {
            if (Tag == null)
                Tag = MediaTag.Create(content.Url);
        }

        public override string? Description => $"{Content?.Name}: {Tag?.Description}, {Tag?.Duration}";
    }
}
