using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core
{
    public class LocalContentProvider
        : IContentProvider
    {
        public async IAsyncEnumerable<Content> GetContents(Content content)
        {
            await foreach(var item in content.GetValues())
            {
                yield return item;
            }
        }

        public IAsyncEnumerable<Content> GetContents(string root)
        {
            return GetContents(new DirectoryContent(root));
        }
    }
}
