using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;

namespace AvpMediaPlayer.Core
{
    public class ContentLoadFactory
    {
        public class ContentUIFactory : IContentLoadFactory
        {
            private readonly IContentRepository _contentRepository;
            private readonly IMediaContentFactory _contentFactory;
            private bool isContentLoaded;

            private Func<Content, bool> Filter { get; }

            public ContentUIFactory(IContentRepository contentRepository
                , IMediaContentFactory contextFactory
                , Func<Content, bool>? filter = null)
            {
                this._contentRepository = contentRepository ?? throw new System.ArgumentNullException(nameof(contentRepository));
                this._contentFactory = contextFactory;
                Filter = filter ?? new(c => c != null);
            }

            public async IAsyncEnumerable<ContentUIModel> Get()
            {
                await CheckLoadContent();

                yield return new ContentUIModel(_contentFactory.Create(_contentRepository.Root), Load);
            }

            public async IAsyncEnumerable<ContentUIModel> Get(Content content)
            {
                await CheckLoadContent();

                foreach (var val in Load(content))
                    yield return val;
                //return _contentRepository.Create(content).Select(c => new ContentUI(c, Create(c))); // Load all tree
            }

            private async Task CheckLoadContent()
            {
                if (isContentLoaded) return;

                isContentLoaded = true;
                await _contentRepository.LoadContents();

            }

            private IEnumerable<ContentUIModel> Load(Content content)
            {
                return _contentRepository.Get(content)
                    .Where(Filter)
                    .Select(c => new ContentUIModel(_contentFactory.Create(c), Load)); // Deffered loading
            }
        }
    }
}
