using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AvpMediaPlayer.Core.Models
{
    public abstract class BaseUIModel<TModel> : INotifyPropertyChanged
        where TModel : class
    {
        protected BaseUIModel()
        {
        }
        public BaseUIModel(TModel? model)
        {
            Model = model;
        }

        public TModel? Model { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<TValue>(ref TValue property, TValue value, [CallerMemberName] string? propertyName = null)
        {
            if (property?.Equals(value) == true) return;
            property = value;
            OnPropertyChanged(propertyName);
        }
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
