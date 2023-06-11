using System.Collections.Generic;
using System;
using System.Windows.Controls;
using System.Windows;
using System.IO;
using System.Text;
using System.Windows.Documents;
using System.Windows.Markup;

namespace TaskManager.Client.Controls;

public class ExtendedRichTextBox : RichTextBox
{
    private static List<Guid> _recursionProtection = new List<Guid>();

    public static readonly DependencyProperty DocumentXamlProperty = DependencyProperty.RegisterAttached(
        "DocumentXaml",
        typeof(string),
        typeof(ExtendedRichTextBox),
        new FrameworkPropertyMetadata(
            "",
            FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
            OnPropertyChanged_DocumentXaml
        )
    );

    public static string GetDocumentXaml(DependencyObject obj)
    {
        return (string)obj.GetValue(DocumentXamlProperty);
    }

    public static void SetDocumentXaml(DependencyObject obj, string value)
    {
        var frameworkElement = (FrameworkElement)obj;
        if (frameworkElement.Tag is null || (Guid)frameworkElement.Tag == Guid.Empty)
        {
            frameworkElement.Tag = Guid.NewGuid();
        }
            
        _recursionProtection.Add((Guid)frameworkElement.Tag);
        obj.SetValue(DocumentXamlProperty, value);
        _recursionProtection.Remove((Guid)frameworkElement.Tag);
    }

    private static void OnPropertyChanged_DocumentXaml(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
    {
        var richTextBox = (RichTextBox)dependencyObject;
        if (richTextBox.Tag != null && _recursionProtection.Contains((Guid)richTextBox.Tag))
        {
            return;
        }

        FlowDocument? flowDocument = new();
        try
        {
            string docXaml = GetDocumentXaml(richTextBox);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(docXaml));
            
            if (!string.IsNullOrEmpty(docXaml))
            {
                flowDocument = (FlowDocument)XamlReader.Load(stream);
            }
        }
        catch (Exception)
        {
        }
        richTextBox.Document = flowDocument;

        #warning Potential memory leak
        richTextBox.TextChanged += OnTextChanged;
    }

    private static void OnTextChanged(object obj, TextChangedEventArgs e)
    {
        if (obj is not RichTextBox richTextBox)
        {
            return;
        }

        SetDocumentXaml(richTextBox, XamlWriter.Save(richTextBox.Document));
    }
}
