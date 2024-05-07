using Newtonsoft.Json.Linq;
using Panuon.WPF.UI;
using System.Text.Json.Nodes;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;

namespace RFCL.Views;

public partial class LogsWindow : WindowX
{
    private JObject jsonObject = JObject.Parse(File.ReadAllText("RFCL.json"));
    public static LogsWindow logsWindow;

    public LogsWindow()
    {
        WindowXCaption.SetBackground(this, new SolidColorBrush
        {
            Color = (Color)ColorConverter.ConvertFromString("#2A2A2A"),
            Opacity = 0.5
        });
        logsWindow = this;
        InitializeComponent();
        Theme();
    }
    private void Theme()
    {
        if (jsonObject["Background"] == null)
        {
            jsonObject["Background"] = new JObject
            {
                { "Acrylic", true },
                { "AcrylicColor", "#00FFFFFF" }
            };
        }

        string acrylicColor = (string)jsonObject["Background"]["AcrylicColor"];
        string imageUrl = (string)jsonObject["Background"]["Image"];

        try
        {
            if ((bool)jsonObject["Background"]["Acrylic"])
            {
                Effect = new AcrylicWindowXEffect { AccentColor = (Color)ColorConverter.ConvertFromString(acrylicColor) };
            }
            else if (!string.IsNullOrEmpty(imageUrl))
            {
                Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(imageUrl)),
                    Opacity = (double)(jsonObject["Background"]["ImageOpacity"] ?? 1.0)
                };
            }
            else
            {
                Effect = new AcrylicWindowXEffect { AccentColor = (Color)ColorConverter.ConvertFromString(acrylicColor) };
            }
        }
        catch
        {
            Effect = new AcrylicWindowXEffect { AccentColor = (Color)ColorConverter.ConvertFromString(acrylicColor) };
        }
    }
}