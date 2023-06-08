using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace TaskManager.Client.Extensions;

internal static class ViewExtensions
{
    public static T? FindAncestor<T>(this DependencyObject dependencyObject)
        where T : FrameworkElement
    {
        ArgumentNullException.ThrowIfNull(dependencyObject);

        DependencyObject? parentElement = VisualTreeHelper.GetParent(dependencyObject);
        T? control = parentElement.FindControlOrAncestor<T>();
        return control;
    }

    public static T? FindControlOrAncestor<T>(this DependencyObject dependencyObject)
        where T : FrameworkElement
    {
        ArgumentNullException.ThrowIfNull(dependencyObject);

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

    public static T? FindChild<T>(this DependencyObject dependencyObject) 
        where T : FrameworkElement
    {
        return FindChild<T>(dependencyObject, child => true);
    }

    public static T? FindChild<T>(this DependencyObject dependencyObject, Predicate<T> predicate)
        where T : FrameworkElement
    {
        ArgumentNullException.ThrowIfNull(dependencyObject);

        int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

            if (child is T typedChild && predicate(typedChild))
            {
                return typedChild;
            }

            T? foundChild = FindChild<T>(child, predicate);

            if (foundChild != null)
            {
                return foundChild;
            }
        }

        return null;
    }

    public static void ForEachChild<T>(this DependencyObject dependencyObject, Action<T> action)
        where T : FrameworkElement
    {
        ArgumentNullException.ThrowIfNull(dependencyObject);

        int childrenCount = VisualTreeHelper.GetChildrenCount(dependencyObject);

        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

            if (child is T typedChild)
            {
                action(typedChild);
                return;
            }

            ForEachChild<T>(child, action);
        }
    }

    public static T? FindControl<T>(this Visual visual, Point point)
        where T : FrameworkElement
    {
        HitTestResult hitResult = VisualTreeHelper.HitTest(visual, point);

        if (hitResult is null || hitResult?.VisualHit is null)
        {
            return null;
        }

        var dependencyObject = hitResult.VisualHit as DependencyObject;
        if (dependencyObject is null)
        {
            return null;
        }

        return dependencyObject.FindControlOrAncestor<T>();
    }

    public static TResult? FindUnderlyingControl<TResult, TDrag>(this Visual visual, Point point, TDrag draggingControl)
        where TResult : FrameworkElement
        where TDrag : FrameworkElement
    {
        FrameworkElement? dependencyObject = null;

        VisualTreeHelper.HitTest(visual, null, new HitTestResultCallback(result =>
        {
            DependencyObject? visualHit = result.VisualHit;

            TDrag? visualAncestor = visualHit.FindControlOrAncestor<TDrag>();
            if (visualAncestor == draggingControl)
            {
                return HitTestResultBehavior.Continue;
            }

            FrameworkElement? underlyingControl = visualHit as FrameworkElement;

            if (underlyingControl != null)
            {
                dependencyObject = underlyingControl;
                return HitTestResultBehavior.Stop;
            }

            return HitTestResultBehavior.Continue;
        }), new PointHitTestParameters(point));

        if (dependencyObject is null)
        {
            return null;
        }

        return dependencyObject.FindControlOrAncestor<TResult>();
    }

    public static IEnumerable<T> FindChildren<T>(this DependencyObject parent)
    {
        int childrenCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childrenCount; i++)
        {
            DependencyObject child = VisualTreeHelper.GetChild(parent, i);

            if (child is T typedChild)
            {
                yield return typedChild;
            }

            foreach (T nestedChild in FindChildren<T>(child))
            {
                yield return nestedChild;
            }
        }
    }
}
