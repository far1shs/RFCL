using MinecraftLaunch.Classes.Interfaces;
using MinecraftLaunch.Classes.Models.Game;
using MinecraftLaunch.Classes.Models.Launch;
using MinecraftLaunch.Components.Authenticator;
using MinecraftLaunch.Components.Checker;
using MinecraftLaunch.Components.Launcher;
using MinecraftLaunch.Components.Resolver;
using Panuon.WPF.UI;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace RFCL.Views;

public partial class HomeView : Page
{
    public static HomeView homeView;

    public HomeView()
    {
        homeView = this;
        InitializeComponent();

        Loaded += (x, s) =>
        {
            var resolver = new GameResolver(".minecraft").GetGameEntitys();
            GameComboBox.Items.Clear();
            foreach (var item in resolver)
            {
                GameComboBox.Items.Add(item.Id);
            }
            GameComboBox.SelectedIndex = 0;
        };

        AccountComboBox.Items.Add(new { Image = "C:\\Users\\heliu\\Pictures\\b_2f6fec3508a160d59c72998ae0bd7a34.jpg", Name = "Far1sh" });
    }

    private async void LaunchGameClick(object sender, RoutedEventArgs e)
    {
        LogsWindow logsWindow = new LogsWindow();
        logsWindow.Show();
        await Task.Run(() =>
        {
            var authenticator = new OfflineAuthenticator("Far1sh").Authenticate();
            var resolver = new GameResolver(".minecraft");

            //MicrosoftAuthenticator authenticator = new("我是返回值");

            //YggdrasilAuthenticator authenticator = new("https://littleskin.cn/api/yggdrasil", "heliuliua@163.com", "132546jia");
            //var userProfile = await authenticator.AuthenticateAsync();

            var config = new LaunchConfig
            {
                //Account = authenticator.AuthenticateAsync().Result,
                Account = authenticator,
                IsEnableIndependencyCore = true,
                JvmConfig = new("C:\\Program Files\\Microsoft\\jdk-17.0.10.7-hotspot\\bin\\javaw.exe")
                {
                    MaxMemory = 2048,
                },
                LauncherName = "RFCL"
            };

            Dispatcher.Invoke(async () =>
            {
                Toast.Show($"正在验证资源");
                IChecker resourceChecker = new ResourceChecker(resolver.GetGameEntity(GameComboBox.SelectedItem.ToString()));
                var result = await resourceChecker.CheckAsync();
                if (!result)
                {
                    Toast.Show($"有资源缺失,正在补全 {resourceChecker}");
                }

                Launcher launcher = new(resolver, config);
                var gameProcessWatcher = await launcher.LaunchAsync(GameComboBox.SelectedItem.ToString());

                Toast.Show($"正在启动 {GameComboBox.SelectedItem}");

                gameProcessWatcher.OutputLogReceived += (sender, args) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        LogsWindow.logsWindow.Logs.Text += args.Text + "\n";
                        LogsWindow.logsWindow.Logs.ScrollToEnd();
                    });
                };

                gameProcessWatcher.Exited += (sender, args) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        LogsWindow.logsWindow.Logs.Text += $"[{DateTime.Now.ToString("HH:mm:ss")}] [RFCL/INFO] Game Exit";
                        LogsWindow.logsWindow.Logs.ScrollToEnd();
                        Toast.Show($"游戏 {GameComboBox.SelectedItem} 已退出");
                    });
                };
            });
        }).ConfigureAwait(false);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var button = new Button
        {
            Content = "关闭"
        };
        button.Click += (s, e) => Service.Contentdialog.Close();
        Service.Contentdialog.Show(new StackPanel
        {
            Children =
                {
                    new TextBlock { Text = "123" },
                    button
                }
        });
    }

    private async void Button_Click_1(object sender, RoutedEventArgs e)
    {
        //MicrosoftAuthenticator authenticator = new("1ed83b46-66a9-41ca-b14f-26e82a6be3a5");
        //await authenticator.DeviceFlowAuthAsync(dc =>
        //{
        //    Process.Start("explorer.exe", dc.VerificationUrl);
        //    Debug.WriteLine(dc.UserCode);
        //});
        //var userProfile = await authenticator.AuthenticateAsync();

        YggdrasilAuthenticator authenticator = new("https://littleskin.cn/api/yggdrasil", "heliuliua@163.com", "132546jia");
        var userProfile = await authenticator.AuthenticateAsync();
        foreach (var item in userProfile)
        {
            Debug.WriteLine(item);
        }
    }
}