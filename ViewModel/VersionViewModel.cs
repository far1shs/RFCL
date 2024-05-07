using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MinecraftLaunch.Components.Resolver;
using Newtonsoft.Json.Linq;
using RFCL.Model;
using System.IO;
using System.Windows.Controls;

namespace RFCL.ViewModel;

public partial class VersionViewModel : ObservableObject
{
    private JObject jsonObject = JObject.Parse(File.ReadAllText("RFCL.json"));

    [ObservableProperty]
    public List<VersionModel.VersionSourceList> versionSourceList = new();

    [ObservableProperty]
    public VersionModel.VersionSourceList versionSourceItem = new();

    [ObservableProperty]
    public List<VersionModel.VersionPathSourceList> versionPathSourceList = new();

    [ObservableProperty]
    public VersionModel.VersionPathSourceList versionPathSourceItem = new();

    [ObservableProperty]
    public bool loading = true;

    public VersionViewModel()
    {
        foreach (var item in jsonObject["Version"]["Path"])
        {
            VersionPathSourceList.Add(new VersionModel.VersionPathSourceList
            {
                Id = (int)item["Id"],
                Name = (string)item["Name"],
                Path = (string)item["Path"],
            });
        }

        var VersionRes = new GameResolver(".minecraft").GetGameEntitys();
        foreach (var item in VersionRes)
        {
            VersionSourceList.Add(new VersionModel.VersionSourceList { Id = item.Id });
        }

        VersionPathSourceItem = VersionPathSourceList.FirstOrDefault(vp => vp.Id == (int)jsonObject["Version"]["Select"]);
        Loading = false;
    }

    [RelayCommand]
    public void RefreshCommand()
    {
        Loading = true;
        VersionPathSourceList.Clear();
        VersionSourceList.Clear();
        foreach (var item in JObject.Parse(File.ReadAllText("RFCL.json"))["VersionPath"])
        {
            VersionPathSourceList.Add(new VersionModel.VersionPathSourceList
            {
                Id = (int)item["Id"],
                Name = (string)item["Name"],
                Path = (string)item["Path"],
            });
        }

        var VersionRes = new GameResolver(".minecraft").GetGameEntitys();
        foreach (var item in VersionRes)
        {
            VersionSourceList.Add(new VersionModel.VersionSourceList { Id = item.Id });
        }
        Loading = false;
    }
}