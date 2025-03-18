namespace AvpMediaPlayer.Media.Interfaces
{
    public interface IMediaManagement
    {
        public double Volume { get; set; }
        public bool LoopTrack { get; set; }
        public bool LoopCatalog { get; set; }
        public double Duration { get; }
        public double Position { get; set; }

        void CallDurationChange();
        void CallPositionChange();
    }
}
