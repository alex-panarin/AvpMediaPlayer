using AvpMediaPlayer.Core.Interfaces;
using TagLib;
using TagMediaTypes = TagLib.MediaTypes;
namespace AvpMediaPlayer.Media.Models
{

    public class MediaTag : IMediaTag
    {
        public static MediaTag? Create(string path)
        {
            try
            {
                return new MediaTag(path);
            }
            catch (UnsupportedFormatException)
            {
                throw;// TODO Debug
            }
        }

        private readonly TagLib.File _File;

        protected MediaTag(string path)
        {
            _File = TagLib.File.Create(path);
        }

        public Core.Interfaces.MediaTypes? MediaType
        {
            get
            {
                return _File?.Properties?.MediaTypes switch
                {
                    TagMediaTypes.Video => Core.Interfaces.MediaTypes.Video,
                    TagMediaTypes.Audio => Core.Interfaces.MediaTypes.Audio,
                    TagMediaTypes.Photo => Core.Interfaces.MediaTypes.Photo,
                    _ => Core.Interfaces.MediaTypes.Unknown,
                };
            }
        }
        
        public string? Album => _File?.Tag.Album;
        public string? Artists => string.Join("; ", _File!.Tag.AlbumArtists);
        public string? Composers => string.Join("; ", _File!.Tag.Composers);
        public string? Genres => string.Join("; ", _File!.Tag.Genres);
        public string? Year => _File?.Tag.Year.ToString();
        public string? Title => _File?.Tag.Title;
        public int? BitsPerSample => _File?.Properties.BitsPerSample;
        public int? BitRate => _File?.Properties.AudioBitrate;
        public int? Channels => _File?.Properties.AudioChannels;
        public int? SampleRate => _File?.Properties.AudioSampleRate;
        public string? Description => _File?.Properties.Description;
        public int? PhotoHeight => _File?.Properties.PhotoHeight;
        public int? PhotoWidth => _File?.Properties.PhotoWidth;
        public int? PhotoQuaity => _File?.Properties.PhotoQuality;
        public TimeSpan? Duration => _File?.Properties.Duration;
        protected Properties? Properties => _File?.Properties;
    }
}
