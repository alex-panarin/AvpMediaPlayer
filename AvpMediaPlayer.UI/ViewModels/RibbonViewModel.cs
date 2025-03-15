using AvpMediaPlayer.Core.Helpers;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public enum EmptyState
    {
        Empty,
        Single,
        Full
    }
    public class RibbonViewModel : ObservableObject
    {
        private string? _SelectedText;
        private EmptyState _IsEmpty;
        private ContentUIModel? _SelectedItem;
        private readonly string[] _allwaysVisible = { RibbonModel.List };

        public RibbonViewModel()
        {
            Buttons = [];
        }
        public RibbonViewModel(Action<RibbonModel?> buttonClick)
        {
            Buttons = [..RibbonModel.Buttons.Select(b =>
            {
                return new RibbonModel(b, buttonClick);
            })];
            
            ChangeButtons();
        }
        public ContentUIModel? SelectedItem 
        { 
            get => _SelectedItem;
            set
            {
                SetProperty(ref _SelectedItem, value);
                SelectedText = _SelectedItem?.Title;
            }
        }
        public EmptyState IsEmpty { get => _IsEmpty; set => SetProperty(ref _IsEmpty, value); }
        public string? SelectedText { get => _SelectedText; private set => SetProperty(ref _SelectedText, value); }
        public ObservableCollection<RibbonModel> Buttons { get; }
        private void ChangeButtons()
        {
            var visible = SelectedItem is not null
                && SelectedItem.IsDirectory == false;
            
            visible = true;

            var allButtons = new Func<bool>(() => Buttons
                .Where(b => _allwaysVisible.Any(a => a == b.Action ) == false)
                .All(b => b.IsVisible == visible));

            //if (allButtons())
            //    return;

            Buttons.ForEach(b =>
            {
                //if (b.Action != RibbonModel.List)
                    b.IsVisible = visible;
            });

            IsEmpty = allButtons() && visible ? EmptyState.Full : EmptyState.Single;

            OnPropertyChanged(nameof(Buttons));
        }
    }
}
