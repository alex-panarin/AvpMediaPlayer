using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core
{
    public class LocalContentProvider
        : IContentProvider
    {
        public Content GetContent(string url)
        {
            return Directory.Exists(url)
                ? new DirectoryContent(url)
                : new FileContent(url);
        }

        public IEnumerable<Content> GetContents(string url)
        {
            var flags = new[]
            {
                FileAttributes.Hidden,
                FileAttributes.System,
                FileAttributes.ReparsePoint
            };
           
            foreach (var path in (Directory.Exists(url)
                ? Directory.EnumerateFileSystemEntries(url, "*.*", SearchOption.TopDirectoryOnly)
                : [])
                .Where(p =>
                {
                    var attr = File.GetAttributes(p);
                    return flags.All(f => attr.HasFlag(f) == false);
                }))
            {
                yield return File.GetAttributes(path).HasFlag(FileAttributes.Directory)
                     ? new DirectoryContent(path)
                     : new FileContent(path);
            }
        }
    }
}
