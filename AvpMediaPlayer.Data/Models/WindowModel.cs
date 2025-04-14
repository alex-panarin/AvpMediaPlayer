namespace AvpMediaPlayer.Data.Models
{
    public class WindowModel : Entity
    {
        public enum State
        {
            CenterScreen,
            CenterOwner,
            Manual
        }
        public WindowModel()
        {
            Name = "window";
        }

        public State WindowState { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
