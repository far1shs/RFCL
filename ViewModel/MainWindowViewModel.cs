using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RFCL.Views;
using System.Windows.Media;

namespace RFCL.ViewModel;

public partial class MainWindowViewModel : ObservableObject
{
    private static MainWindowViewModel mainWindowViewModel;
    private HomeView homeView = new();
    private GameView gameView = new();
    private VersionView versionView = new();
    private DownloadView downloadView = new();
    private AccountView accountView = new();
    private SettingsView settingsView = new();

    private static SolidColorBrush black = new SolidColorBrush
    {
        Color = (Color)ColorConverter.ConvertFromString("#2A2A2A"),
        Opacity = 0.5
    };

    private static SolidColorBrush transparent = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF"));

    [ObservableProperty]
    public static SolidColorBrush homeViewBackground = black;

    [ObservableProperty]
    public static SolidColorBrush gameViewBackground = transparent;

    [ObservableProperty]
    public static SolidColorBrush versionViewBackground = transparent;

    [ObservableProperty]
    public static SolidColorBrush downloadViewBackground = transparent;

    [ObservableProperty]
    public static SolidColorBrush accountViewBackground = transparent;

    [ObservableProperty]
    public static SolidColorBrush settingsViewBackground = transparent;

    [ObservableProperty]
    public static object frameContent = null!;

    public MainWindowViewModel()
    {
        mainWindowViewModel = this;
        FrameContent = new HomeView();
    }

    public static void ViewToggle(object view)
    {
        mainWindowViewModel.FrameContent = view;
    }

    private void AllBackground()
    {
        HomeViewBackground = transparent;
        GameViewBackground = transparent;
        VersionViewBackground = transparent;
        DownloadViewBackground = transparent;
        AccountViewBackground = transparent;
        SettingsViewBackground = transparent;
    }

    [RelayCommand]
    public void HomeViewCommand()
    {
        FrameContent = homeView;
        AllBackground();
        HomeViewBackground = black;
    }

    [RelayCommand]
    public void GameViewCommand()
    {
        FrameContent = gameView;
        AllBackground();
        GameViewBackground = black;
    }

    [RelayCommand]
    public void VersionViewCommand()
    {
        FrameContent = versionView;
        AllBackground();
        VersionViewBackground = black;
    }

    [RelayCommand]
    public void DownloadViewCommand()
    {
        FrameContent = downloadView;
        AllBackground();
        DownloadViewBackground = black;
    }

    [RelayCommand]
    public void AccountViewCommand()
    {
        FrameContent = accountView;
        AllBackground();
        AccountViewBackground = black;
    }

    [RelayCommand]
    public void SettingsViewCommand()
    {
        FrameContent = settingsView;
        AllBackground();
        SettingsViewBackground = black;
    }
}