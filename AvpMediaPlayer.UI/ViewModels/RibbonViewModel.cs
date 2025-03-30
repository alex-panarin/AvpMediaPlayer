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
        private ContentUIModel? _SelectedItem;
        private readonly string[] _allwaysVisible =
        {
            RibbonModel.List,
            RibbonModel.AddTrack,
            RibbonModel.AddList,
            RibbonModel.NewList,
            RibbonModel.Sets
        };
        private readonly IEnumerable<RibbonModel> _changableButtons;

        public RibbonViewModel()
        {
            Buttons = [];
            _changableButtons = [];
        }
        public RibbonViewModel(Action<RibbonModel?> buttonClick)
        {
            Buttons = [..RibbonModel.Buttons.Select(b =>
            {
                return new RibbonModel(b, buttonClick);
            })];

            Buttons
                .ForEach((b, i) =>
                {
                    b.IsVisible = i == 1 || i == 5 || _allwaysVisible.Any(a => a == b.Action);
                });

            _changableButtons = Buttons
                .Where(b => b.IsVisible != true);

            ChangeButtons();
        }
        public ContentUIModel? SelectedItem 
        { 
            get => _SelectedItem;
            set
            {
                SetProperty(ref _SelectedItem, value);
                SelectedText = _SelectedItem?.Title;
                ChangeButtons();
            }
        }
        public string? SelectedText 
        { 
            get => _SelectedText; 
            private set => SetProperty(ref _SelectedText, value); 
        }
        public ObservableCollection<RibbonModel> Buttons { get; }
        private void ChangeButtons()
        {
            var visible = SelectedItem is not null
                && SelectedItem.IsDirectory == false;
            
            var allButtons = new Func<bool>(() => _changableButtons
                .All(b => b.IsVisible == visible));

            if (allButtons()) return;

            _changableButtons.ForEach(b =>
            {
                 b.IsVisible = visible;
            });

            //IsEmpty = allButtons() && visible ? EmptyState.Full : EmptyState.Single;
            OnPropertyChanged(nameof(Buttons));
        }
    }
}
