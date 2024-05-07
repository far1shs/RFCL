using Newtonsoft.Json.Linq;
using Panuon.WPF.UI;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RFCL;

public partial class MainWindow : WindowX
{
    private JObject jsonObject = JObject.Parse(File.ReadAllText("RFCL.json"));
    public static MainWindow mainWindow = null!;

    public MainWindow()
    {
        WindowXCaption.SetBackground(this, new SolidColorBrush
        {
            Color = (Color)ColorConverter.ConvertFromString("#2A2A2A"),
            Opacity = 0.5
        });
        mainWindow = this;
        InitializeComponent();
        string updatedJsonString = jsonObject.ToString();
        File.WriteAllText("RFCL.json", updatedJsonString);
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