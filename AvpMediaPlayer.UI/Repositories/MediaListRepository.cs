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

        public MediaListModel AddOrUpdate(MediaListModel mediaList, string[] urls)
        {
            var dbList = _dataContext.Add(mediaList.Title!, urls);
            
            foreach (var content in _contentUIFactory.Get(urls))
            {
                if (content.IsDirectory)
                {
                    mediaList.Contents.AddRange(_contentUIFactory.Get(content.Model!));
                }
                else
                {
                    mediaList.Contents.Add(content);
                }
            }

            return mediaList;
        }

        public void Rename(string? oldName, MediaListModel listModel)
        {
            if (oldName is null
                || oldName == listModel.Title
                || listModel.Title is null)
            {
                return;
            }
            
            var list = _dataContext.Get(oldName);
            if(list == null)
            {
                list = _dataContext.Add(listModel.Title!, [.. listModel.Contents.Select(x => x.Model!.Url)]);
            }
            else
            {
                _dataContext.Rename(oldName, listModel.Title!);
            }
        }

        public void Delete(MediaListModel list)
        {
            if (list?.Title is null) return;

            _dataContext.Delete(list.Title);
        }
    }
}
