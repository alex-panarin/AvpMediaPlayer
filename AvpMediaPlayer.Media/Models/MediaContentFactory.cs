using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Media.Models
{
    public class MediaContentFactory : IMediaContentFactory
    {
        public IMediaContent Create(Content content)
        {
            if (content.IsDirectory)
                return new ContainerMediaContent(content);

            var tag = MediaTag.Create(content.Url);
            
            return tag?.MediaType switch
            {
                MediaTypes.Video => new VideoMediaContent(content, tag),
                MediaTypes.Audio => new AudioMediaContent(content, tag),
                MediaTypes.Photo => new PhotoMediaContent(content, tag),
                _ => throw new ArgumentOutOfRangeException("MediaType")
            };
        }
    }
}
