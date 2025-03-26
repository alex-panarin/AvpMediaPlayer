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
        private readonly Func<bool> _isMediaInit;
        private readonly Func<int> _getStream;
        private double _Volume = 0.5d;
        private bool _LoopTrack = false;
        private bool _LoopCatalog = false;
        public AudioMediaManagement(Func<bool> isMediaInit, Func<int> getStream)
        {
            _isMediaInit = isMediaInit;
            _getStream = getStream;
        }

        public double Volume 
        { 
            get => _Volume; 
            set 
            {
                if (!IsInitialize 
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
                if (!IsInitialize || (value
                    ? !Bass.ChannelAddFlag(Stream, BassFlags.Loop) 
                    : !Bass.ChannelRemoveFlag(Stream, BassFlags.Loop)))
                    return;

                _LoopTrack = value;
            } 
        }
        public bool LoopCatalog 
        { 
            get => _LoopCatalog;
            set => _LoopCatalog = value;
        }
        public double Duration
        {
            get => !IsInitialize
                ? 0d
                : Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetLength(Stream));
        }
        public double Position 
        {
            get => !IsInitialize 
                ? 0d
                : Bass.ChannelBytes2Seconds(Stream, Bass.ChannelGetPosition(Stream));
            set => Bass.ChannelSetPosition(Stream, Bass.ChannelSeconds2Bytes(Stream, value));
        }
        private int Stream => _getStream();
        private bool IsInitialize  => _isMediaInit();

        public event PropertyChangedEventHandler? PropertyChanged;

        public void CallDurationChange()
        {
            OnPropertyChanged(nameof(Duration));
        }

        public void CallPositionChange()
        {
            OnPropertyChanged(nameof(Position));
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new(propertyName));
        }
        
    }
}
