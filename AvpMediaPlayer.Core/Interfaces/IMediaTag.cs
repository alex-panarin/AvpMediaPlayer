
namespace AvpMediaPlayer.Core.Interfaces
{
    public enum MediaTypes
    {
        Unknown,
        Audio, 
        Video,
        Photo
    }
    public interface IMediaTag
    {
        string? Album { get; }
        string? Artists { get; }
        int? BitRate { get; }
        int? BitsPerSample { get; }
        int? Channels { get; }
        string? Composers { get; }
        string? Description { get; }
        string? Genres { get; }
        MediaTypes? MediaType { get; }
        int? PhotoHeight { get; }
        int? PhotoQuaity { get; }
        int? PhotoWidth { get; }
        int? SampleRate { get; }
        TimeSpan? Duration { get; }
        string? Title { get; }
        string? Year { get; }
    }
}
