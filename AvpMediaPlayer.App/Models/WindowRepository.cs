using Avalonia;
using Avalonia.Controls;
using AvpMediaPlayer.Data;
using AvpMediaPlayer.Data.Models;
using static AvpMediaPlayer.Data.Models.WindowModel;

namespace AvpMediaPlayer.App.Models
{
    public class WindowRepository : IWindowRepository
    {
        private readonly IWindowDataContext _dataContext;

        private WindowModel? Model { get; }
        const string dbname = "medialist.db";
        const string name = "window";
        public WindowRepository()
        {
            _dataContext = new WindowDataContext(dbname, name);
            Model = _dataContext.Model ?? new WindowModel
            {
                WindowState = State.CenterScreen,
            };
        }

        public WindowStartupLocation GetState
        {
            get{ return ToState(Model!);}
        }
        public PixelPoint Location => new PixelPoint(Model!.X, Model.Y);
        public void Save(WindowStartupLocation state, PixelPoint location) 
        {
            Model!.X = location.X; Model.Y = location.Y;

            _dataContext.Model = Model;
        }
        private WindowStartupLocation ToState(WindowModel model) => model.WindowState switch
        {
            State.CenterScreen => WindowStartupLocation.CenterScreen,
            State.CenterOwner => WindowStartupLocation.CenterOwner,
            _ => WindowStartupLocation.Manual
        };
    }
}
