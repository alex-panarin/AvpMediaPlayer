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
        private double _Volume = 0.5d;
        private bool _LoopTrack = false;
        public AudioMediaManagement()
        {
        }

        public double Volume
        {
            get => _Volume;
            set
            {
                if (Stream == 0
                    || !Bass.ChannelSetAttribute(Stream, ChannelAttribute.Volume, value))
                    return;

                _Volume = value;
            }
        }

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
            get => Stream == 0 ? 0d : Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetLength(Stream));
        }

        public double Position
        {
            get => Stream == 0 ? 0d : Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetPosition(Stream));
            set => Bass.ChannelSetPosition(Stream, Bass.ChannelSeconds2Bytes(Stream, value));
        }

        private int Stream { get;  set; }

        public void CallDurationChange()
        {
            OnPropertyChanged(nameof(Duration));
        }

        public void CallPositionChange()
        {
            OnPropertyChanged(nameof(Position));
        }

        public void SetStream(int stream)
        {
            Stream = stream;    
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
