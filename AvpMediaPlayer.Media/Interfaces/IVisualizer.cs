namespace AvpMediaPlayer.Media.Interfaces
{
    public enum VisualType
    {
        BarSingle,
        LinearSingle,
        BarDouble,
        LineDouble
    }
    public record VisualInfo(int Points, double Level);
    public interface IVisualizer
    {
        VisualInfo? Info { get; }
        void VisualizeChannels(double[][] channels);
        void VisualizeLevels(double[] levels);
        void PlaceVisualize(IVisualize? visualize);
    }

    public interface IVisualize
    {
        Action<double[][]> ViewChannels { get; }
        Action<double[]> ViewLevels { get; }
    }
}
