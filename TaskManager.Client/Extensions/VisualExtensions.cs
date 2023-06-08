using System.Windows;
using System.Windows.Media;
using TaskManager.Client.Model;

namespace TaskManager.Client.Extensions;

internal static class VisualExtensions
{
    public static Point RelativeToMainWindow(this Visual visual)
    {
        return visual.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
    }

    public static ControlDimensions GetControlDimensions(this FrameworkElement frameworkElement)
    {
        return new ControlDimensions
        {
            TopLeft = frameworkElement.RelativeToMainWindow(),
            Height = frameworkElement.ActualHeight,
            Width = frameworkElement.ActualWidth
        };
    }
}
