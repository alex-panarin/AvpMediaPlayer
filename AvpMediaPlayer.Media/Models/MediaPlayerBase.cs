using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Media.Interfaces;
using System.Timers;

namespace MediaConsole.Media.Models
{
    public abstract class MediaPlayerBase
        : IMediaPlayer
        , IDisposable
    {
        public static int TIMER_MS = 100;
        private bool _disposedValue;
        private IMediaContent? _MediaContent;
        private readonly System.Timers.Timer _timer = new(TIMER_MS) { AutoReset = true };
        public IMediaContent? MediaContent { get => _MediaContent; set => OnSetMediaContent(value); }
        protected MediaPlayerBase()
        {
            _timer.Elapsed += _timerTick;
        }
        public PlayerState State { get; protected set; }
        protected IVisualizer? Visualizer { get; private set; }
        public abstract IMediaManagement MediaManagement { get; protected set; }
        protected void StopTimer() => _timer.Stop();
        void IMediaPlayer.Pause()
        {
            StopTimer();
            OnPause();
            State = PlayerState.Pause;
        }
        void IMediaPlayer.Play()
        {
            OnPlay();
            State = PlayerState.Play;
            _timer.Start();
        }
        void IMediaPlayer.Stop()
        {
            StopTimer();
            OnStop();
            State = PlayerState.Stop;
        }
        void IMediaPlayer.SetVisualizer(IVisualizer visualizer)
        {
            Visualizer = visualizer;
        }
        protected abstract void OnPlay();
        protected abstract void OnPause();
        protected abstract void OnStop();
        private void _timerTick(object? sender, ElapsedEventArgs e)
        {
            OnTimerCallback();
        }
        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    ManagedDispose();
                }

                UnmanagedDispose();
                _disposedValue = true;
            }
        }
        protected virtual void OnSetMediaContent(IMediaContent? value)
        {
            if(State == PlayerState.Play)
                ((IMediaPlayer)this).Stop();
            _MediaContent = value;
        }
        protected virtual void ManagedDispose()
        {
            _timer.Stop();
            _timer.Elapsed -= _timerTick;
            _timer.Dispose();
        }
        protected virtual void OnTimerCallback()
        {
            
        }
        protected virtual void UnmanagedDispose()
        {
        }
        ~MediaPlayerBase()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
