using ManagedBass;
using ManagedBass.FftSignalProvider;

namespace AvpMediaPlayer.Media.Interfaces
{
    public class AudioVisualData
        : IMediaData
    {
        private int _stream;
        private readonly int _points;
        private readonly double _scale;
        private readonly SignalProvider? _signalProvider;
        const int defaultPoints = 1024;
        public AudioVisualData()
            :this(200, 64)
        {
            
        }
        public AudioVisualData(double scale, int pounts = 0)
        {
            _signalProvider = new SignalProvider(DataFlags.FFT1024, true, true) { WindowType = WindowType.Hanning };
            _points = pounts;
            _scale = scale;
        }

        public void ClearStream()
        {
            _stream = 0;
        }
        public void SetStream(int stream)
        {
            _stream = stream; 
            _signalProvider?.SetChannel(stream);
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
        public double[][] Spectrums
        { 
            get
            {
                return _stream == 0 
                    ? [new double[0], new double[0]] 
                    : [.. _signalProvider!.DataSampleWindowed
                        .Select((c, n) => 
                            c.DivideToParts(_points == 0 ? defaultPoints : _points)
                            .AdjustToScale(1, _scale, true, out _).Data)];
            }
        }
        public double[] Levels => [LeftLevel, RightLevel];
    }
}
