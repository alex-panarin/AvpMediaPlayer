namespace AvpMediaPlayer.Media.Interfaces
{
    public interface IMediaManagement
    {
        public double Volume { get; set; }
        public bool LoopTrack { get; set; }
        public bool LoopCatalog { get; set; }
        public TimeSpan Duration { get; }
        public TimeSpan Position { get; set; }

        void CallDurationChange();
        void CallPositionChange();
    }
}
