using Avalonia.Controls;
using Avalonia.Threading;
using AvpMediaPlayer.Core.Interfaces;
using AvpMediaPlayer.Core.Models;
using AvpMediaPlayer.UI.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace AvpMediaPlayer.UI.ViewModels
{
    public class ContentViewModel : ObservableObject
    {
        private readonly IContentLoadFactory? _contentFactory;
        public bool _IsPaneOpen = true;
        private ContentUIModel? _SelectedItem;
        private ObservableCollection<ContentUIModel>? _Roots;
        private ObservableCollection<ContentUIModel>? _Contents;
        private Stack<KeyValuePair<ContentUIModel, ObservableCollection<ContentUIModel>>> _history = [];
        private bool _IsWaitLoad = true;
        private string? _SelectedText;
        
        public ContentViewModel()
        {
            //OnPrevRoot = new(() =>
            //{
            //    IsWaitLoad = true;

            //    Task.Run(() =>
            //    {
            //        var hasValue = _history.TryPop(out var content);

            //        if (hasValue)
            //        {
            //            _Contents = content.Value;
            //        }

            //        Dispatcher.UIThread.Invoke(() =>
            //        {
            //            IsWaitLoad = false;
                        
            //            if (hasValue)
            //            {
            //                SelectedItem = content.Key;
            //                OnPropertyChanged(nameof(Contents));
            //            }

            //            OnPropertyChanged(nameof(IsPrevVisible));
            //        });
            //    });
            //});
        }
        public ContentViewModel(IContentLoadFactory contentFactory)
            : this()
        {
            _contentFactory = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
            //RibbonViewModel = new(OnRibbonButtonClick);
        }
        public ObservableCollection<ContentUIModel>? Roots 
        { 
            get
            {
                if (_Roots is not null)
                    return _Roots;

                IsWaitLoad = true;

                Task.Run(async () =>
                {
                    _Roots = new ObservableCollection<ContentUIModel>();

                    await foreach (var content in _contentFactory!.Get())
                    {
                        _Roots.Add(content);
                    }

                    Dispatcher.UIThread.Invoke(() =>
                    {
                        IsWaitLoad = false;
                        OnPropertyChanged(nameof(Roots));
                    });
                });

                return _Roots;
            }
        }
        public ObservableCollection<ContentUIModel>? Contents
        {
            get
            {
                if (_Contents is not null)
                    return _Contents;

                LoadContents();

                return _Contents;
            }
        }
       
        public string? SelectedText
        { 
            get => _SelectedText; 
            private set => SetProperty(ref _SelectedText, value); 
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
        public bool IsWaitLoad 
        { 
            get { return _IsWaitLoad; } 
            set => SetProperty(ref _IsWaitLoad, value); 
        }
        internal void LoadContents()
        {
            IsWaitLoad = true;
            
            Task.Run(async () =>
            {
                if (_Roots is null)
                {
                    _Roots = [];

                    await foreach (var content in _contentFactory!.Get())
                    {
                        _Roots.Add(content);
                    }
                }
                // Nether set SelectedItem properties in ListBox
                var root = SelectedItem ?? _Roots.FirstOrDefault();
                
                if(SelectedItem is not null)
                    _history.Push(new(root!, _Contents!));

                if (root is not null)
                {
                    _Contents = [];

                    foreach (var content in root.Contents!)
                    {
                        _Contents.Add(content);
                    }

                    Dispatcher.UIThread.Invoke(() =>
                    {
                        IsWaitLoad = false;
                        OnPropertyChanged(nameof(Contents));
                        OnPropertyChanged(nameof(IsPrevVisible));
                    });
                }
            });
        }
        public bool IsPrevVisible => _history.Any();
        
        internal protected void OnRibbonButtonClick(RibbonModel? model)
        {
            switch(model?.Action)
            {
                case RibbonModel.List:
                    //OnPrevRoot.Execute(null);
                    break;
                case RibbonModel.Stop:
                    //MediaModel?.Stop();
                    break;
                case RibbonModel.Play:
                    //MediaModel?.Play();
                    break;
                case RibbonModel.Pause:
                    //MediaModel?.Pause();
                    break;
                case RibbonModel.Show:
                    //model?.ShowPopup?.Invoke(true);
                    //VisualWindow.ShowWindow(_owner!, this);
                    break;
                default:
                    break;
            }
        }
    }
}

