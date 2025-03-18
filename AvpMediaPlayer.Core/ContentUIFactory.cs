 using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core
{
    public class ContentUIFactory : IContentUIFactory
    {
        private readonly IContentRepository _contentRepository;
        private readonly IMediaContentFactory _contentFactory;
        private Func<Content, bool> Filter { get; }
        public ContentUIFactory(IContentRepository contentRepository
            , IMediaContentFactory contextFactory
            , Func<Content, bool> filter)
        {
            this._contentRepository = contentRepository ?? throw new System.ArgumentNullException(nameof(contentRepository));
            this._contentFactory = contextFactory;
            Filter = filter ?? new(c => c != null);
        }
        public IEnumerable<ContentUIModel> Get(IEnumerable<string> paths)
        {
            var contents = paths.Select(path => _contentRepository.Get(path))
                .SelectMany(c => Get(c));
            return contents;
        }
        public IEnumerable<ContentUIModel> Get(Content content)
        {
            foreach (var val in Load(content))
                yield return val;
        }
        private IEnumerable<ContentUIModel> Load(Content content)
        {
            if (content.IsDirectory)
            {
                return _contentRepository.Get(content)
                    .Where(Filter)
                    .Select(c => new ContentUIModel(_contentFactory.Create(c), Load)); // Deffered loading
            }
            return [new ContentUIModel(_contentFactory.Create(content), Load)];
        }
    }
}
