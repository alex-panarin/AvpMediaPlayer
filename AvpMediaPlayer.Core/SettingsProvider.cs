using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AvpMediaPlayer.Core
{
    public class SettingsProvider : ISettingsProvider
    {
        private readonly JsonSerializerOptions _options
            = new() { PropertyNameCaseInsensitive = true } ;
        private readonly string _path;

        public SettingsProvider(string fileName)
        {
            _path = Path.Combine(AppContext.BaseDirectory, fileName);
        }

        public SettingsModel? Get()
        {
            using var fileStream = File.Open(_path, FileMode.OpenOrCreate);
            try
            {
                return JsonSerializer.Deserialize<SettingsModel>(fileStream, _options);
            }
            catch 
            { 
                var model = new SettingsModel(); 
                SaveInternal(model, fileStream);

                return model;   
            }
        }

        private void SaveInternal(SettingsModel model, FileStream fileStream)
        {
            try
            {
                var content = JsonSerializer.Serialize(model, _options);
                var bytes = Encoding.UTF8.GetBytes(content);
                
                fileStream.Write(bytes, 0, bytes.Length);
            }
            catch { }
        }

        public void Save(SettingsModel? model)
        {
            if(File.Exists(_path))
                File.Delete(_path);

            using var fileStream = File.Open(_path, FileMode.CreateNew);
            SaveInternal(model!, fileStream);
        }
    }
}
