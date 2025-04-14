using Avalonia;
using Avalonia.Controls;

namespace AvpMediaPlayer.App.Models
{
    public interface IWindowRepository
    {
        WindowStartupLocation GetState { get; }
        PixelPoint Location { get; }

        void Save(WindowStartupLocation state, PixelPoint location);
    }
}