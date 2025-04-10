using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using System.Diagnostics;

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
            _contentRepository = contentRepository ?? throw new System.ArgumentNullException(nameof(contentRepository));
            _contentFactory = contextFactory;
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
            foreach (var val in Load(content)
                .Where(c => c?.Model is not null && Filter(c.Model) == true))
            {
                yield return val;
            }
        }

        private IEnumerable<ContentUIModel> Load(Content content)
        {
            if (content.IsDirectory)
            {
                return _contentRepository.Get(content)
                    .SelectMany(c => Load(c)); 
                // (new ContentUIModel(_contentFactory.Create(c), Load)); // Deffered loading
            }
            try
            {
                return [new ContentUIModel(_contentFactory.Create(content), Load)];
            }
            catch (Exception ex)
            {
                Debug.Write($"ContentUIFactory.Load Error: {ex}");
            }
            return Enumerable.Empty<ContentUIModel>();
        }
    }
}
