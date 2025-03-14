using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        private ContentUIModel? _SelectedItem;

        public NavigationViewModel()
        {
            Ribbon = new(OnButtonClick);
            Container = new();
        }

        public RibbonViewModel Ribbon { get; }
        public MediaContainerViewModel Container { get; }

        public ContentUIModel? SelectedItem 
        { 
            get => _SelectedItem;
            set
            {
                SetProperty(ref _SelectedItem, value);
                Ribbon.SelectedItem = _SelectedItem;
                Container.SelectedItem = _SelectedItem;
            } 
        }

        private void OnButtonClick(RibbonModel? model)
        {
            switch (model?.Action)
            {
                case RibbonModel.Back:
                    break;
                case RibbonModel.Stop:
                    break;
                case RibbonModel.Play:
                    break;
                case RibbonModel.Pause:
                    break;
                case RibbonModel.Show:
                    break;
                default:
                    break;
            }
        }

    }
}
