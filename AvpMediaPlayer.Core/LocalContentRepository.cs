using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using System.Collections.Concurrent;

namespace AvpMediaPlayer.Core
{
    public class LocalContentRepository 
        : IContentRepository
    {
        private readonly IContentProvider _provider;
        private readonly ConcurrentDictionary<string, Content> _storage = [];
        public LocalContentRepository(IContentProvider provider)
        {
            this._provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public IEnumerable<Content> Get(Content parent)
        {
            LoadContents(parent);
            return parent.Contents;
        }

        public Content Get(string path)
        {
            var content = _storage.GetOrAdd(path, _provider.GetContent(path));
            LoadContents(content);

            return content;
        }

        private void LoadContents(Content parent)
        {
            foreach (var content in _provider.GetContents(parent.Url))
            {
                if (_storage.TryGetValue(content.Url, out var temp))
                    break;
                
                temp = _storage.GetOrAdd(content.Url, content);
                parent.Contents.Add(temp);
                LoadContents(temp);
            }
        }
    }
}
