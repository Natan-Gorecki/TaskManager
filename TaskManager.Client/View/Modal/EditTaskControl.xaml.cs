using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using TaskManager.Client.Utils;
using TaskManager.Core.Extensions;

namespace TaskManager.Client.View.Modal;

/// <summary>
/// Interaction logic for EditTaskControl.xaml
/// </summary>
public partial class EditTaskControl : UserControl
{
    private readonly ILogger<EditTaskControl> _logger = App.IoC.GetRequiredService<ILogger<EditTaskControl>>();
    private readonly IModalPageManager _modalPageManager = App.IoC.GetRequiredService<IModalPageManager>();

    public static readonly DependencyProperty EditedTaskProperty = DependencyProperty.Register(
    "EditedTask", typeof(Task), typeof(EditTaskControl), new PropertyMetadata(null));

    public Task EditedTask
    {
        get { return (Task)GetValue(EditedTaskProperty); }
        set 
        { 
            SetValue(EditedTaskProperty, value);
            var task = new Task();
            DataContext = task.CopyFrom(value);
        }
    }

    public EditTaskControl()
    {
        InitializeComponent();
    }

    private void CloseModalPage(object sender, RoutedEventArgs e)
    {
        _modalPageManager.Close();
    }

    private void SaveTask(object sender, RoutedEventArgs e)
    {
        if (EditedTask is null)
        {
            _logger.LogCritical($"EditedTask is null.");
            return;
        }

        if (DataContext is not Task coreTask)
        {
            _logger.LogCritical($"EditTaskControl data context is null or has different type than {typeof(Task)}.");
            return;
        }

        EditedTask.Name = coreTask.Name;
        EditedTask.Description = coreTask.Description;
        EditedTask.Status = coreTask.Status;

        _modalPageManager.Close();
    }
}
