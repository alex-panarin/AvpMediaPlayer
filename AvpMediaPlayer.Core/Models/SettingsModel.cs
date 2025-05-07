namespace AvpMediaPlayer.Core.Models
{
    public class SettingsModel
    {
        public bool LoopCatalog { get; set; } = true;
        public bool LoopTrack { get; set; } = false;
        public bool LoopLists { get; set; } = true;

        public string? LastList { get; set; }
        public string? LastTrack { get; set; }
    }
}
