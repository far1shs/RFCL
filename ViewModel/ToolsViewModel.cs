using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace RFCL.ViewModel;

public class ToolsViewModel : ObservableObject
{
    public ICommand EditJsonCommand { get; }

    public ToolsViewModel()
    {
        EditJsonCommand = new RelayCommand<object>(editJsonCommand);
    }

    private void editJsonCommand(object path)
    {
    }
}