using System.Windows;

namespace RFCL.Service;

internal class Contentdialog
{
    private static MainWindow Main = MainWindow.mainWindow;

    public static Contentdialog? Show(UIElement uIElement)
    {
        Main.ContentdialogContent.Children.Add(uIElement);
        Main.Contentdialog.Visibility = Visibility.Visible;
        Main.Shadowing.Visibility = Visibility.Visible;
        return null;
    }

    public static Contentdialog? Close()
    {
        Main.Contentdialog.Visibility = Visibility.Collapsed;
        Main.Shadowing.Visibility = Visibility.Collapsed;
        Main.ContentdialogContent.Children.Clear();
        return null;
    }
}