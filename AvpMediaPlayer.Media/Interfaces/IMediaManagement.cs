namespace AvpMediaPlayer.Media.Interfaces
{
    public interface IMediaManagement
    {
        public double Volume { get; set; }
        public bool LoopTrack { get; set; }
        public bool LoopCatalog { get; set; }
        public bool LoopLists { get; set; }
        public double Duration { get; }
        public double Position { get; set; }

        void SetStream(int stream);
        void CallDurationChange();
        void CallPositionChange();
    }
}
