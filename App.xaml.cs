using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;

namespace RFCL;

public partial class App : Application
{
    private JObject jsonObject = JObject.Parse(File.ReadAllText("RFCL.json"));

    private App()
    {
        if (!File.Exists("RFCL.json")) File.WriteAllText("RFCL.json", "{}");
        Version();
    }

    private void Version()
    {
        if (jsonObject["Version"] == null)
        {
            jsonObject["Version"] = new JObject();
            jsonObject["Version"]["Select"] = 1;
            jsonObject["Version"]["Path"] = new JArray();
            JArray versionArray = (JArray)jsonObject["Version"]["Path"];
            if (versionArray.Count == 0)
            {
                var official = new JObject
            {
                { "Id", 0 },
                { "Name", "官方启动器目录" },
                { "Path", $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\.minecraft" }
            };

                var local = new JObject
            {
                { "Id", 1 },
                { "Name", "当前启动器目录" },
                { "Path", $"{Directory.GetCurrentDirectory()}\\.minecraft" }
            };

                versionArray.Add(official);
                versionArray.Add(local);
            }
            File.WriteAllText("RFCL.json", jsonObject.ToString());
        }
    }
}