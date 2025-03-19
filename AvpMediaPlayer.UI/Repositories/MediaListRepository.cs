using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Data;
using AvpMediaPlayer.UI.Models;

namespace AvpMediaPlayer.UI.Repositories
{
    public class MediaListRepository
        : IMediaListRepository
    {
        const string dbname = "medialist.db";
        private readonly IDataContext _dataContext;
        private readonly IContentUIFactory _contentUIFactory;

        public MediaListRepository(IContentUIFactory contentUIFactory)
        {
            _dataContext = new DataContext(dbname);
            _contentUIFactory = contentUIFactory;
        }

        public MediaListModel[] Get()
        {
            return _dataContext.Get()
                .Select(x => new MediaListModel
                {
                    Title = x.Name,
                    Contents = [.. _contentUIFactory.Get(x.Urls)]
                })
                .ToArray();
        }
        public void Add(MediaListModel listModel)
        {
            _dataContext.Add(listModel.Title!, listModel.Contents
                .Select(z => z.Model?.Url!)
                .ToArray());
        }

        public MediaListModel New(string[] urls)
        {
            var list = new MediaListModel();
            foreach (var content in _contentUIFactory.Get(urls))
            {
                list.Title ??= content.IsDirectory
                    ? content.Title
                    : content.Model?.ParentName;

                if (content.IsDirectory)
                {
                    list.Contents.AddRange(_contentUIFactory.Get(content.Model!));
                }
                else
                {
                    list.Contents.Add(content);
                }
            }

            _dataContext.Add(list.Title!, list.Contents.Select(x => x.Model!.Url).ToArray());
            return list;
        }
    }
}
