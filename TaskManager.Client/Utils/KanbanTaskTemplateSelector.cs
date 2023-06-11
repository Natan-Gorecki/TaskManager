using System.Windows;
using System.Windows.Controls;

namespace TaskManager.Client.Utils;

internal class KanbanTaskTemplateSelector : DataTemplateSelector
{
    public DataTemplate? ItemTemplate { get; set; }
    public DataTemplate? ItemPreviewTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        if (item is Task task && task.IsPreview == true)
        {
            return ItemPreviewTemplate;
        }
        else
        {
            return ItemTemplate;
        }
    }
}
