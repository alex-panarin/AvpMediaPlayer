using AvpMediaPlayer.Media.Interfaces;
using ManagedBass;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvpMediaPlayer.Media.Audio
{
    public class AudioMediaManagement
        : IMediaManagement
        , INotifyPropertyChanged
    {
        private double _Volume = 0.01d;
        private bool _LoopTrack = false;
        private string? _durationText;
        private string? _positionText;
        private object _locker = new object();

        public AudioMediaManagement()
        {
        }

        public double Volume
        {
            get => _Volume;
            set
            {
                if (Stream == 0) return;
                //var volume = Bass.ChannelGetAttribute(Stream, ChannelAttribute.Volume);
                if (Bass.ChannelSetAttribute(Stream, ChannelAttribute.Volume, value))
                    _Volume = value;
                OnPropertyChanged(nameof(VolumeText));
            }
        }

        public string VolumeText => _Volume.ToString("F2");

        public bool LoopTrack
        {
            get => _LoopTrack;
            set
            {
                if (Stream == 0 || (value
                    ? !Bass.ChannelAddFlag(Stream, BassFlags.Loop)
                    : !Bass.ChannelRemoveFlag(Stream, BassFlags.Loop)))
                    return;

                _LoopTrack = value;
            }
        }

        public bool LoopCatalog { get; set; } = true;

        public bool LoopLists { get; set; } = true;

        public double Duration
        {
            get
            {
                var duration = 0d;
                if(Stream != 0)
                    duration = Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetLength(Stream));
                _durationText = string.Format("{0:F0}m:{1:F0}s", duration / 60, duration % 60);
                return duration;
            }
        }

        public double Position
        {
            get
            {
                lock (_locker)
                {
                    var position = 0d;
                    if (Stream != 0)
                        position = Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetPosition(Stream));

                    _positionText = string.Format("{0:F0}m:{1:F0}s", position /60, position % 60);
                    return position;
                }
            }
            set
            {
                if (Stream == 0) return;

                lock (_locker)
                {
                    Bass.ChannelPause(Stream);
                    Bass.ChannelSetPosition(Stream, Bass.ChannelSeconds2Bytes(Stream, value));
                    Bass.ChannelPlay(Stream);
                }
            }
        }

        private int Stream { get;  set; }

        public string Timings  => $"{_positionText} - {_durationText}";

        public void CallDurationChange()
        {
            OnPropertyChanged(nameof(Duration));
        }

        public void CallPositionChange()
        {
            OnPropertyChanged(nameof(Position));
            OnPropertyChanged(nameof(Timings));
        }

        public void SetStream(int stream)
        {
            Stream = stream;
            Volume = _Volume;
            CallDurationChange();
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
