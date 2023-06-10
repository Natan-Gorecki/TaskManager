using System.Windows;
using System.Windows.Controls;

namespace TaskManager.Client.View.Modal;

/// <summary>
/// Interaction logic for EditTaskControl.xaml
/// </summary>
public partial class EditTaskControl : UserControl
{
    public EditTaskControl()
    {
        InitializeComponent();
    }

    private void CloseModalPage(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is not MainWindow mainWindow)
        {
            return;
        }

        mainWindow.modalPage.Visibility = Visibility.Collapsed;
    }
}
