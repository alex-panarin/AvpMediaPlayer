using AvpMediaPlayer.Data.Models;

namespace AvpMediaPlayer.Data
{
    public interface IWindowDataContext
    {
        WindowModel? Model { get; set; }
    }
}
