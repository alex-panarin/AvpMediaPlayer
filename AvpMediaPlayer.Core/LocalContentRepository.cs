using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core
{
    public class LocalContentRepository 
        : IContentRepository
    {
        private readonly IContentProvider provider;
        private readonly DirectoryContent _rootDirectory;

        public LocalContentRepository(IContentProvider provider, string rootPath)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _rootDirectory = new DirectoryContent(rootPath);
        }

        public IEnumerable<Content> Get(Content parent)
        {
            return parent.Contents;
        }

        public async Task LoadContents()
        {
            await LoadContents(_rootDirectory);
        }
        
        private async Task LoadContents(Content parent)
        {
            await foreach (Content content in provider.GetContents(parent))
            {
                parent.Contents.Add(content);

                await LoadContents(content);
            }
        }
    }
}
