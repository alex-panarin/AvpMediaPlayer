using AvpMediaPlayer.Media.Interfaces;
using ManagedBass;
using ManagedBass.FftSignalProvider;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvpMediaPlayer.Media.Audio
{
    public class AudioVisualData
        : IMediaData
        , INotifyPropertyChanged
    {
        private int _stream;
        private readonly int _points;
        private readonly double _scale;
        private readonly SignalProvider? _signalProvider;
        const int defaultPoints = 128;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new(propertyName));

        public AudioVisualData()
            :this(80, 20, false)
        {
        }
        public AudioVisualData(double scale, int pounts = 0, bool stereo = true)
        {
            _signalProvider = new SignalProvider(DataFlags.FFT256, true, stereo) { WindowType = WindowType.Hanning };
            _points = pounts;
            _scale = scale;
        }

        public void ClearStream()
        {
            _stream = 0;
            InvokeUpdate();
        }
        public void SetStream(int stream)
        {
            _stream = stream; 
            _signalProvider?.SetChannel(stream);
        }
        public void InvokeUpdate()
        {
            OnPropertyChanged(nameof(Spectrums));
            OnPropertyChanged(nameof(Levels));
        }

        protected double LeftLevel
        {
            get
            {
                var leftLevel = _stream == 0 ? 0d : Bass.ChannelGetLevelLeft(_stream);
                return (double)leftLevel / short.MaxValue;

            }

        }
        protected double RightLevel
        {
            get
            {
                var rightLevel = _stream == 0 ? 0d : Bass.ChannelGetLevelRight(_stream);
                return (double)rightLevel / short.MaxValue;
            }
        }
        public double[] Spectrum
        {
            get { 
                    return _stream == 0
                        ? Enumerable.Range(0, _points).Select(p => 5d).ToArray()
                        : _signalProvider!.DataSampleWindowed.First()
                            .DivideToParts(_points == 0 ? defaultPoints : _points)
                                .AdjustToScale(0, _scale, true, out _).Data;
                }
        }
        public double[][] Spectrums
        { 
            get
            {
                return _stream == 0
                    ? [[..Enumerable.Range(0, _points).Select(p => p * 10d)], [..Enumerable.Range(0, _points).Select( p => p * 20d)]]
                    : [.. _signalProvider!.DataSampleWindowed
                        .Select(channelData => 
                            channelData.DivideToParts(_points == 0 ? defaultPoints : _points)
                            .AdjustToScale(0, _scale, true, out _).Data)];
            }
        }
        public double[] Levels => [LeftLevel != 0 ? LeftLevel : 5d , RightLevel != 0 ? RightLevel : 5d];
        public int Points => _points;
        public double Height => _scale;

    }
}
