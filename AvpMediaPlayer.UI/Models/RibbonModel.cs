﻿using CommunityToolkit.Mvvm.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AvpMediaPlayer.UI.Models
{   
    public class RibbonModel : INotifyPropertyChanged
    {
        public const string Play =  "play";
        public const string Stop =  "stop";
        public const string Next =  "next";
        public const string Prev =  "prev";
        public const string Pause = "pause";
        public const string List =  "back";
        public const string Show =  "show";
        public const string Sets = "sets";
        public const string NewList = "open";
        public const string AddList = "addfolder";
        public const string AddTrack = "addfiles";
        public const string Separator = "separator";

        public static (string, string, string)[] Buttons =
        {
            (List, "apps_list_regular", "Список"),
            (Separator, "", ""),
            (NewList, "folder_open_regular", "Открыть"),
            (AddList, "folder_add_regular", "Папку"),
            (AddTrack, "collections_add_regular", "Файлы"),
            (Separator, "", ""),
            (Sets, "settings_regular", "Настройки"),
            (Separator, "", ""),
            (Stop, "stop_regular", "Стоп"),
            (Play, "play_regular", "Играть"),
            (Pause, "pause_regular", "Пауза"),
            (Prev, "caret_left_regular", "Назад"),
            (Next, "caret_right_regular", "Вперед"),
            (Separator, "", ""),
            (Show, "device_eq_regular", "Просмотр"),
           
        };
        private bool? _IsEnabled = true;
        private bool? _IsVisible = false;

        public RibbonModel((string, string, string) val, Action<RibbonModel> command)
        {
            ButtonClick = new(b => command(b!));
            (Action, ImageKey, ToolTip) = val;
        }

        public string? ImageKey { get; }

        public string? Action { get; }

        public string? ToolTip { get; }

        public bool IsSeparator => Action == Separator;

        public RelayCommand<RibbonModel> ButtonClick { get; }

        public bool? IsEnabled
        { 
            get => _IsEnabled; 
            internal set => SetProperty(ref _IsEnabled, value); 
        }

        public bool? IsVisible 
        { 
            get => _IsVisible; 
            internal set => SetProperty(ref _IsVisible, value); 
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void SetProperty<T>(ref T property, T value, [CallerMemberName] string? propName = null)
        {
            property = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
