using AvpMediaPlayer.Media.Interfaces;
using ManagedBass;
using MediaConsole.Media.Models;
using System.Runtime.InteropServices;

namespace AvpMediaPlayer.Media.Audio
{
    public class AudioPlayer
        : MediaPlayerBase
    {
        private static Dictionary<int, PluginInfo>? _plugins;
        private string[]? _SupportedFormats;
        private int _Stream = 0;
        private readonly SynchronizationContext? _syncContext;
        private readonly INavigation _navigation;

        static AudioPlayer()
        {
            InitializeDevice();
        }
        private static void InitializeDevice()
        {
            IsInitialized = Bass.Init();

            if (IsInitialized == false)
                throw new InvalidOperationException(Bass.LastError.ToString());

            _plugins = new[]
            {
                OSPlatform.Windows,
                OSPlatform.Linux,
                OSPlatform.OSX
            }
            .Where(op => RuntimeInformation.IsOSPlatform(op))
            .SelectMany(op =>
            {
                var dll = new[]
                {
                    (nameof (OSPlatform.Linux),  "libbass*.so"),
                    (nameof(OSPlatform.OSX), "libbass*.dylib"),
                    (nameof(OSPlatform.Windows), "bass*.dll")
                }
                .FirstOrDefault(lib => lib.Item1.Equals(op.ToString(), StringComparison.InvariantCultureIgnoreCase))
                .Item2;
                
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                var excludes = new[]
                {
                    Path.Combine(directory, dll.Replace("*", "")),
                    Path.Combine(directory, dll.Replace("*", "_fx"))
                };

                return Directory.EnumerateFiles(directory, dll, SearchOption.TopDirectoryOnly)
                    .Except(excludes);
            })
            .Select(file =>
            {
                var handle = Bass.PluginLoad(file);
                return (handle, (handle == 0 ? default : Bass.PluginGetInfo(handle)));
            })
            .Where(p => p.Item1 != 0)
            .ToDictionary(p => p.Item1, p => p.Item2);
        }

        public string[] SupportedFormats => _SupportedFormats ??=
            Bass.SupportedFormats.Split(";")
            .Union(_plugins!
            .Select(p => string.Join("; ", $"{p.Value.Formats[0].FileExtensions}")))
            .ToArray();
        public AudioPlayer(IMediaManagement mediaManagement, IVisualizer visualizer, INavigation navigation) 
            : base()
        {
            _syncContext = SynchronizationContext.Current;
            MediaManagement = mediaManagement;
            Visualizer = visualizer;
            _navigation = navigation;
        }

        protected int Stream
        { 
            get => _Stream; 
            private set
            {
                if (!Bass.ChannelGetInfo(value, out var info))
                    throw new ArgumentException("Invalid Channel Handle: " + value);
                _Stream = value;
                
                Bass.ChannelSetSync(_Stream, SyncFlags.End | SyncFlags.Mixtime, 0, GetSyncProcedure(() =>
                {
                    try
                    {
                        if (MediaManagement?.LoopTrack == false 
                            && !Bass.ChannelHasFlag(_Stream, BassFlags.Loop))
                        {
                            StopTimer();
                            Visualizer?.ClearStream();
                            State = PlayerState.Stop;
                            if (MediaManagement.LoopCatalog)
                            {
                                _Stream = 0;
                                _navigation.NavigateNextItem(1);
                            }
                        }
                    }
                    finally { }
                }));
            } 
        } 
        public static bool IsInitialized { get; private set; }
        protected override void OnPause()
        {
            if (Stream == 0 || State == PlayerState.Pause) return;

            Bass.ChannelPause(Stream);
        }
        protected override void OnPlay()
        {
            if (State == PlayerState.Play) return;

            if (Stream == 0)
            {
                Stream = Bass.CreateStream(MediaContent!.Content!.Url);
                if (Stream == 0)
                    throw new OperationCanceledException($"Error Create: {Bass.LastError}");
                
                if (Visualizer is not null)
                {
                    MediaManagement?.CallDurationChange();
                    MediaManagement?.SetStream(Stream);
                    Visualizer.SetStream(Stream);
                }
            }

            if (Stream != 0)
            {
                if(!Bass.ChannelPlay(Stream))
                    throw new OperationCanceledException($"Error Play: {Bass.LastError}");
            }
        }
        protected override void OnStop()
        {
            if (Stream == 0 || State == PlayerState.Stop) return;
            
            Bass.ChannelStop(Stream);
            Bass.StreamFree(Stream);
            _Stream = 0;
        }
        protected override void OnTimerCallback()
        {
            _syncContext?.Post((cb) =>
            {
                if (State == PlayerState.Play)
                {
                    MediaManagement?.CallPositionChange();
                    Visualizer?.Visualize();
                }
                if (State == PlayerState.Stop)
                    Visualizer?.ClearStream();
            }, null);
        }
        protected override void UnmanagedDispose()
        {
            Bass.Free();
        }
        private SyncProcedure GetSyncProcedure(Action callBack)
        {
            return (SyncHandle, Channel, Data, User) =>
            {
                if (callBack == null)
                    return;

                if (_syncContext == null)
                    callBack();
                else 
                    _syncContext.Post(cb => callBack(), null);
            };
        }
    }
}
