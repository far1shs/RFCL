using MinecraftLaunch.Components.Installer;
using RFCL.ViewModel.Download;
using System.Windows.Controls;
using System.Windows;
using System.Diagnostics;
using Panuon.WPF.UI;
using System.Windows.Media;
using MinecraftLaunch.Components.Resolver;
using static System.Net.Mime.MediaTypeNames;

namespace RFCL.Views.Download;

public partial class InstallView : Page
{
    private InstallViewModel installViewModel;

    public InstallView(string Id)
    {
        installViewModel = new();
        installViewModel.GameName = Id;
        InitializeComponent();
        InitializeInstallation();
    }

    private async void InitializeInstallation()
    {
        Loading.IsActive = true;
        InstallList.Visibility = Visibility.Collapsed;
        ForgeComboBox.IsEnabled = true;
        FabricComboBox.IsEnabled = true;
        await Task.Run(() =>
        {
            Task.Run(async () =>
            {
                var forgebuild = await ForgeInstaller.EnumerableFromVersionAsync(installViewModel.GameName);
                try
                {
                    forgebuild.First();
                    foreach (var item in forgebuild)
                    {
                        Dispatcher.Invoke(() => ForgeComboBox.Items.Add(item.ForgeVersion));
                    }
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        ForgeComboBox.Items.Add("未找到此版本的Forge");
                        //ForgeComboBox.IsEnabled = false;
                    });
                }
                Dispatcher.Invoke(() => ForgeComboBox.SelectedIndex = 0);
            });
            Task.Run(async () =>
            {
                var fabricbuild = await FabricInstaller.EnumerableFromVersionAsync(installViewModel.GameName);
                try
                {
                    fabricbuild.First();
                    FabricComboBox.Items.Add("请选择Fabric");
                    foreach (var item in fabricbuild)
                    {
                        Dispatcher.Invoke(() => FabricComboBox.Items.Add(item.DisplayVersion));
                    }
                }
                catch
                {
                    Dispatcher.Invoke(() =>
                    {
                        FabricComboBox.Items.Add("未找到此版本的Fabric");
                        //FabricComboBox.IsEnabled = false;
                    });
                }
                Dispatcher.Invoke(() => FabricComboBox.SelectedIndex = 0);
            });
        }).ConfigureAwait(false);
        Dispatcher.Invoke(() =>
        {
            Loading.IsActive = false;
            InstallList.Visibility = Visibility.Visible;
        });
    }

    private async void Button_Click(object sender, RoutedEventArgs e)
    {
        var progressBar = new ProgressBar
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A2A2A")),
            Height = 8,
            Value = 0
        };
        var textBox = new TextBox
        {
            Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF")),
            Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF")),
            BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00FFFFFF")),
            Height = 100,
            Margin = new Thickness(0, 10, 0, 10),
            TextWrapping = TextWrapping.Wrap
        };
        Service.Contentdialog.Show(new StackPanel
        {
            Width = 300,
            Children =
            {
                new TextBlock { Text = $"正在安装游戏核心 {installViewModel.GameName}", FontWeight = FontWeights.Bold, FontSize = 14 },
                textBox,
                new Border
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2A2A2A")),
                    Child = progressBar
                }
            }
        });
        Task.Run(async () =>
        {
            var resolver = new GameResolver(".minecraft");
            var vanlliaInstaller = new VanlliaInstaller(resolver, installViewModel.GameName);

            vanlliaInstaller.ProgressChanged += (_, x) =>
            {
                Dispatcher.Invoke(() =>
                {
                    if (textBox.LineCount >= 10)
                    {
                        textBox.Text = textBox.Text.Substring(textBox.Text.IndexOf(Environment.NewLine) + Environment.NewLine.Length);
                    }
                    progressBar.Value = x.Progress * 100;
                    textBox.Text += x.ProgressStatus + "\n";
                    textBox.ScrollToEnd();
                });
            };

            vanlliaInstaller.Completed += (_, x) =>
            {
                Dispatcher.Invoke(() =>
                {
                    Service.Contentdialog.Close();
                    Toast.Show($"游戏核心 {installViewModel.GameName} 安装成功");
                });
            };

            var a = await vanlliaInstaller.InstallAsync();

            if (a == true)
            {
                Service.Contentdialog.Close();
                Toast.Show($"游戏核心 {installViewModel.GameName} 安装成功");
            }
            else
            {
                Service.Contentdialog.Close();
                Toast.Show($"游戏核心 {installViewModel.GameName} 安装失败");
            }
        });
    }
}