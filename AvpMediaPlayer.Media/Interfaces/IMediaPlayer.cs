using AvpMediaPlayer.Core.Interfaces;

namespace AvpMediaPlayer.Media.Interfaces
{
    public enum PlayerState
    {
        Stop,
        Pause,
        Play
    }
    public interface IMediaPlayer
    {
        IMediaManagement MediaManagement { get; }
        IMediaContent? MediaContent { get; set; }
        PlayerState State { get; }
        void Play();
        void Stop();
        void Pause();
        void SetVisualizer(IVisualizer visualizer);
    }
}
