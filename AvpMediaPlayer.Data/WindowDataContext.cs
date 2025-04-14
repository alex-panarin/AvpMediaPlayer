using AvpMediaPlayer.Data.Models;

namespace AvpMediaPlayer.Data
{
    public class WindowDataContext : DataContext, IWindowDataContext
    {
        public WindowDataContext(string connectionString, string dbname) 
            : base(connectionString, dbname)
        {
        }

        public WindowModel? Model 
        {
            get
            {
                return ProcessCommand<WindowModel>(_dbname, (e, el) => { return; });
            }
            set
            {
                if (value is not null)
                    ProcessCommand<WindowModel>(_dbname, (e, el) => { el.Upsert(value); });
            }
        }
    }
}
