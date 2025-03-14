using AvpMediaPlayer.Core.Helpers;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public enum EmptyState
    {
        Empty,
        Single,
        Full
    }
    public class RibbonViewModel : BaseUIModel<ContentViewModel>
    {
        private string? _SelectedText;
        private EmptyState _IsEmpty;
        
        private readonly string[] allwaysVisible = { RibbonModel.Back };

        public RibbonViewModel()
            : base(default)
        {
            Buttons = [];
        }
        public RibbonViewModel(ContentViewModel model, Action<RibbonModel?> buttonClick)
            : base(model)
        {
            if(Model is not null)
                Model.PropertyChanged += Model_PropertyChanged;

            Buttons = [..RibbonModel.Buttons.Select(b =>
            {
                return new RibbonModel(b, buttonClick);
            })];
            
            ChangeButtons();
        }
        
        public EmptyState IsEmpty { get => _IsEmpty; set => SetProperty(ref _IsEmpty, value); }
        public string? SelectedText { get => _SelectedText; private set => SetProperty(ref _SelectedText, value); }
        
        public ObservableCollection<RibbonModel> Buttons { get; }

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.SelectedItem))
            {
                ChangeButtons();
            }
            else if (e.PropertyName == nameof(Model.SelectedText))
            {
                SelectedText = Model?.SelectedText;
            }
            //else if (e.PropertyName == nameof(Model.IsPrevVisible))
            //{
            //    var button = Buttons.FirstOrDefault(b => allwaysVisible.Any(a => a == b.Action ));
            //    if (button is not null)
            //    {
            //        button.IsVisible = Model!.IsPrevVisible;
            //        IsEmpty = button.IsVisible == true ? EmptyState.Single : EmptyState.Empty;
            //    }
            //}
            //else if (e.PropertyName == nameof(MediaModel))
            //{
            //    OnPropertyChanged(nameof(MediaModel));
            //}
        }

        private void ChangeButtons()
        {
            var visible = Model is not null
                && Model.SelectedItem is not null
                && Model.SelectedItem.IsDirectory == false;

            var allButtons = new Func<bool>(() => Buttons
                .Where(b => allwaysVisible.Any(a => a == b.Action ) == false)
                .All(b => b.IsVisible == visible));

            if (allButtons())
                return;

            Buttons!.ForEach(b =>
            {
                if (b.Action != RibbonModel.Back)
                    b.IsVisible = visible;
            });

            IsEmpty = allButtons() && visible ? EmptyState.Full : EmptyState.Single;

            OnPropertyChanged(nameof(Buttons));
        }
    }
}
