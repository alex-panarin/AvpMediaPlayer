namespace AvpMediaPlayer.Core.Models
{
    public class DirectoryContent : Content
    {
        public DirectoryContent(string url)
            : base(url)
        {
            RootPath = Path.GetDirectoryName(Url)!;
            Name = Path.GetFileName(Url);
        }
        public override bool IsDirectory => true;
        public override string RootPath { get;  }
        public override string Name { get;  }
     
        protected override async IAsyncEnumerable<Content> GetContentsAsync()
        {
            await Task.Yield();

            var flags = new[]
            {
                FileAttributes.Hidden,
                FileAttributes.System, 
                FileAttributes.ReparsePoint
            };
            foreach (var path in Directory.EnumerateFileSystemEntries(Url, "*.*", SearchOption.TopDirectoryOnly)
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
