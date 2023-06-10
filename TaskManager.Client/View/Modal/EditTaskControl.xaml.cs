using System.Windows;
using System.Windows.Controls;
using TaskManager.Core.Models;

namespace TaskManager.Client.View.Modal;

/// <summary>
/// Interaction logic for EditTaskControl.xaml
/// </summary>
public partial class EditTaskControl : UserControl
{
    public EditTaskControl()
    {
        InitializeComponent();
        this.DataContext = new Task
        {
            Id = 1234,
            Name = "Invalid URL to be fixed",
            Description = "With my informations it will require also to fix scaling problems.",
            Status = ETaskStatus.Waiting
        };
    }

    private void CloseModalPage(object sender, System.Windows.RoutedEventArgs e)
    {
        if (Application.Current.MainWindow is not MainWindow mainWindow)
        {
            return;
        }

        mainWindow.modalPage.Visibility = Visibility.Collapsed;
    }
}
