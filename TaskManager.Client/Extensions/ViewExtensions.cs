using System.Windows.Media;
using System.Windows;

namespace TaskManager.Client.Extensions;

internal static class ViewExtensions
{
    public static T? FindAncestor<T>(this DependencyObject dependencyObject)
        where T : FrameworkElement
    {
        DependencyObject? parentElement = VisualTreeHelper.GetParent(dependencyObject);
        T? control = parentElement.FindControl<T>();
        return control;
    }

    public static T? FindControl<T>(this DependencyObject dependencyObject)
        where T : FrameworkElement
    {
        DependencyObject depObj = dependencyObject;
        while ((depObj != null) && !(depObj is T))
        {
            depObj = VisualTreeHelper.GetParent(depObj);
        }

        if (depObj == null)
        {
            return null;
        }

        T? control = depObj as T;

        if (control?.DataContext == null)
        {
            return null;
        }

        return control;
    }
}
