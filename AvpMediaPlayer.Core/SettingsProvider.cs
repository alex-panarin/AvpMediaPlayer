using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AvpMediaPlayer.Core
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly JsonSerializerOptions _options
            = new() { PropertyNameCaseInsensitive = true } ;
        private readonly FileStream _fileStream;

        public SettingsProvider(string fileName)
        {
            var path = Path.Combine(AppContext.BaseDirectory, fileName);
            _fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        }

        public SettingsModel? Get()
        {
            try
            {
                return JsonSerializer.Deserialize<SettingsModel>(_fileStream, _options);
            }
            catch 
            { 
                return new SettingsModel(); 
            }
        }
    }
}
