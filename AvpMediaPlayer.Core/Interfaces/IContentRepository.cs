using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core.Interfaces
{
    public interface IContentRepository
    {
        IEnumerable<Content> Get(Content parent);
        Task LoadContents();
        Content Root { get; }
    }
}
