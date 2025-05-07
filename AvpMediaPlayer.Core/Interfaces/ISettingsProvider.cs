using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface ISettingsProvider
    {
        SettingsModel? Get();
        void Save(SettingsModel? settings);
    }
}
