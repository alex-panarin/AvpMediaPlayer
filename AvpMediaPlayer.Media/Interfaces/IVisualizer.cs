namespace AvpMediaPlayer.Media.Interfaces
{
    public enum VisualType
    {
        BarSingle,
        LinearSingle,
        BarDouble,
        LineDouble
    }
    public interface IVisualizer
    {
        void Visualize();
        void SetStream(int stream);
    }
}
