using System.Windows.Controls;

namespace RFCL.Views;

public partial class SettingsView : Page
{
    public SettingsView()
    {
        InitializeComponent();
        Loaded += Loadeds;
    }
    public void Loadeds(object s, object e)
    {

    }
}