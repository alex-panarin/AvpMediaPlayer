using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core
{
    public class LocalContentRepository 
        : IContentRepository
    {
        private readonly IContentProvider provider;
        public LocalContentRepository(IContentProvider provider, string rootPath)
        {
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            Root = new DirectoryContent(rootPath);
        }
        private DirectoryContent Root { get; }
        Content IContentRepository.Root => Root;
        public async Task LoadContents()
        {
            await LoadContents(Root);
        }
        public IEnumerable<Content> Get(Content parent)
        {
            return parent.Contents;
        }
        private async Task LoadContents(Content root)
        {
            await foreach (Content content in provider.GetContents(root))
            {
                root.Contents.Add(content);

                await LoadContents(content);
            }
        }
    }
}
