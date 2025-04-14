using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using AvpMediaPlayer.App.ViewModels;
using AvpMediaPlayer.App.Views;
using AvpMediaPlayer.Core;
using AvpMediaPlayer.App.Models;

namespace AvpMediaPlayer.App;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    const string settingFileName = "avpsettings.json";
    private readonly WindowRepository _windowRepository = new WindowRepository();
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();


            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(new SettingsProvider(settingFileName), _windowRepository),
                Position = _windowRepository.Location
            };

            desktop.MainWindow.Closing += (s, e) => _windowRepository.Save(desktop.MainWindow.WindowStartupLocation, desktop.MainWindow.Position);        }

        base.OnFrameworkInitializationCompleted();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}