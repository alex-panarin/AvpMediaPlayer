
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
        string? Title { get; }
        string? Album { get; }
        string? Artists { get; }
        string? Description { get; }
        MediaTypes? MediaType { get; }
        TimeSpan? Duration { get; }
    }
}
