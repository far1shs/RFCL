using CommunityToolkit.Mvvm.ComponentModel;

namespace RFCL.ViewModel.Download;

public partial class InstallViewModel : ObservableObject
{
    [ObservableProperty]
    public static string gameName;
}