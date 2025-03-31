using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface ISettingsProvider
    {
        SettingsModel? Get();
    }
}
