using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace TaskManager.Client.View.Modal;

/// <summary>
/// Interaction logic for ModalPage.xaml
/// </summary>
[ContentProperty("ModalPageContent")]
public partial class ModalPage : UserControl
{
    public static readonly DependencyProperty ModalPageContentProperty = DependencyProperty.Register(
    "ModalPageContent", typeof(UserControl), typeof(ModalPage), new PropertyMetadata(null));

    public UserControl ModalPageContent
    {
        get { return (UserControl)GetValue(ModalPageContentProperty); }
        set { SetValue(ModalPageContentProperty, value); }
    }

    public ModalPage()
    {
        InitializeComponent();
    }
}
