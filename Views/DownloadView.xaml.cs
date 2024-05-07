using MinecraftLaunch.Classes.Models.Install;
using MinecraftLaunch.Components.Installer;
using RFCL.ViewModel;
using RFCL.Views.Download;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RFCL.Views;

public partial class DownloadView : Page
{
    public DownloadView()
    {
        InitializeComponent();
        UseTheScrollViewerScrolling(DownloadList);
        GetDownloadList();
    }

    private void RefreshClick(object sender, RoutedEventArgs e) => GetDownloadList();

    public void UseTheScrollViewerScrolling(FrameworkElement fElement)
    {
        fElement.PreviewMouseWheel += (sender, e) =>
        {
            var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArg.RoutedEvent = MouseWheelEvent;
            eventArg.Source = sender;
            fElement.RaiseEvent(eventArg);
        };
    }

    private async void GetDownloadList()
    {
        if (DownloadList.Items.Count > 0) DownloadList.Items.Clear();
        Loading.Visibility = Visibility.Visible;
        await Task.Run(async () =>
        {
            var resolver = await VanlliaInstaller.EnumerableGameCoreAsync();

            foreach (var item in resolver)
            {
                Dispatcher.Invoke(async () =>
                {
                    if (IsRelease.IsChecked == true)
                    {
                        if (item.Type.ToString() == "release")
                        {
                            item.Type = "正式版";
                            DownloadList.Items.Add(item);
                        }
                    }
                    if (IsSnapshot.IsChecked == true)
                    {
                        if (item.Type.ToString() == "snapshot")
                        {
                            item.Type = "测试版";
                            DownloadList.Items.Add(item);
                        }
                    }
                    if (IsOldBeta.IsChecked == true)
                    {
                        if (item.Type.ToString() is "old_beta" or "old_alpha")
                        {
                            item.Type = "远古版";
                            DownloadList.Items.Add(item);
                        }
                    }
                    if (IsRelease.IsChecked == false && IsSnapshot.IsChecked == false && IsOldBeta.IsChecked == false)
                    {
                        DownloadList.Items.Add(item);
                    }
                    await Task.Delay(100);
                });
            }
        });
        Loading.Visibility = Visibility.Collapsed;
    }

    private void DownloadList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var listBox = sender as ListBox;
        VersionManifestEntry lbi = (VersionManifestEntry)listBox.SelectedItem;
        listBox.SelectedIndex = -1;
        if (lbi != null)
        {
            InstallView installView = new(lbi.Id);
            MainWindowViewModel.ViewToggle(installView);
        }
    }
}