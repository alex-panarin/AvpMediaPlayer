namespace AvpMediaPlayer.Media.Interfaces
{
    public interface IMediaManagement
    {
         double Volume { get; set; }
         bool LoopTrack { get; set; }
         bool LoopCatalog { get; set; }
         bool LoopLists { get; set; }
         double Duration { get; }
         double Position { get; set; }
         string Timings { get; }
         string VolumeText { get; }   
        void SetStream(int stream);
        void CallDurationChange();
        void CallPositionChange();
    }
}
