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
        IMediaManagement? MediaManagement { get; set; }
        IVisualizer? Visualizer { get; set; }
        IMediaContent? MediaContent { get; set; }
        PlayerState State { get; }
        void Play();
        void Stop();
        void Pause();
    }
}
