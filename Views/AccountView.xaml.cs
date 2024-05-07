using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows.Controls;

namespace RFCL.Views;

public partial class AccountView : Page
{
    private JObject jsonObject = JObject.Parse(File.ReadAllText("RFCL.json"));

    public AccountView()
    {
        InitializeComponent();
        if (jsonObject["Account"] == null)
        {
            jsonObject["Account"] = new JArray();
            string updatedJsonString = jsonObject.ToString();
            File.WriteAllText("RFCL.json", updatedJsonString);
        }
    }
}