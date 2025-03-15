using AvpMediaPlayer.Core.Interfaces;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.Core.Models
{
    public class ContentUIModel : BaseUIModel<Content>
    {
        private readonly IMediaContent? _mediaContent;
        private readonly Func<Content, IEnumerable<ContentUIModel>>? _contentFactory;
        private ObservableCollection<ContentUIModel>? _Contents;
        public ContentUIModel(IMediaContent mediaContent)
            : base(mediaContent.Content)
        {
            _mediaContent = mediaContent;
        }
        public ContentUIModel(IMediaContent mediaContent, Func<Content, IEnumerable<ContentUIModel>> contentFactory)
           : this(mediaContent)
        {
            _contentFactory = contentFactory;
        }
        public ObservableCollection<ContentUIModel>? Contents
        {
            get
            {
                if (_Contents is null
                    && _contentFactory is not null
                    && Model is not null)
                {
                    SetProperty(ref _Contents, [.. _contentFactory(Model)]);
                }

                return _Contents;
            }
        }
        public string? Title { get => Path.GetFileNameWithoutExtension(Model?.Name); }
        public bool IsDirectory => Model?.IsRoot == true;
        public string? Type => IsDirectory ? "" : Path.GetExtension(Model?.Name);
        public string? Duration => _mediaContent?.Tag?.Duration?.ToString(@"hh\:mm\:ss");
    }
}
