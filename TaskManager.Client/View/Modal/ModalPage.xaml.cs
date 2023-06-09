using System.Windows.Controls;

namespace TaskManager.Client.View.Modal;

/// <summary>
/// Interaction logic for ModalPage.xaml
/// </summary>
public partial class ModalPage : UserControl
{
    public ModalPage()
    {
        InitializeComponent();
    }

    private void CloseModalPage(object sender, System.Windows.RoutedEventArgs e)
    {
        Visibility = System.Windows.Visibility.Collapsed;
    }
}
