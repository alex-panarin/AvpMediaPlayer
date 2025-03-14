namespace AvpMediaPlayer.Core.Models
{
    public class FileContent : Content
    {
        public FileContent(string url)
            : base(url)
        {
            Name = Path.GetFileName(Url);
            RootPath = Path.GetDirectoryName(Url)!;
            Ext = Path.GetExtension(Url);
        }
        public override string Name { get; }
        public override string RootPath { get; } 
        public string Ext { get; }
     
        protected override async IAsyncEnumerable<Content> GetContentsAsync()
        {
            await Task.Yield();

            yield break;
        }
        
    }
}
