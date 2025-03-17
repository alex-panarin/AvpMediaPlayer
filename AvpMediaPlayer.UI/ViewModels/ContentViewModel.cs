using Avalonia.Controls;
using Avalonia.Threading;
using AvpMediaPlayer.Core.Helpers;
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
        private ObservableCollection<ContentUIModel> _Lists;
        private bool _IsWaitLoad = true;
        private string? _SelectedText;

        public ContentViewModel()
        {
            _Lists = [];
        }
        public ContentViewModel(IContentLoadFactory contentFactory)
            : this()
        {
            _contentFactory = contentFactory ?? throw new ArgumentNullException(nameof(contentFactory));
        }
        public ObservableCollection<ContentUIModel> Lists
        {
            get
            {
                if (_Lists.IsNotEmpty())
                    return _Lists;

                IsWaitLoad = true;

                Task.Run(async () =>
                {
                    
                    try
                    {
                        await foreach (var content in _contentFactory!.Get(null)) // TODO:
                        {
                            _Lists.Add(content);
                        }
                    }
                    finally
                    {
                        Dispatcher.UIThread.Invoke(() =>
                        {
                            IsWaitLoad = false;
                            OnPropertyChanged(nameof(Lists));
                        });
                    }
                });

                return _Lists;
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
    }
}

