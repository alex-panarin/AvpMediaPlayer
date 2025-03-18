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
        public override string RootPath { get;  }
        public override string Name { get;  }
        
    }
}
