using System;
using System.Diagnostics;
using System.IO.Packaging;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace TaskManager.Client.Extensions;

internal static class WpfExtensions
{
    public static void LoadViewFromUri(this UserControl userControl, string baseUri)
    {
        try
        {
            var resourceLocater = new Uri(baseUri, UriKind.Relative);
            var resourceOrContentPartInfo = typeof(Application).GetMethod("GetResourceOrContentPart", BindingFlags.NonPublic | BindingFlags.Static);
            ArgumentNullException.ThrowIfNull(resourceOrContentPartInfo);
            
            var exprCa = (PackagePart?)resourceOrContentPartInfo.Invoke(null, new object[] { resourceLocater });
            ArgumentNullException.ThrowIfNull(exprCa);
            var stream = exprCa.GetStream();

            var appBaseUriInfo = typeof(BaseUriHelper).GetProperty("PackAppBaseUri", BindingFlags.Static | BindingFlags.NonPublic);
            ArgumentNullException.ThrowIfNull(appBaseUriInfo);

            var appBaseUri = (Uri?)appBaseUriInfo.GetValue(null, null);
            ArgumentNullException.ThrowIfNull(appBaseUri);

            var uri = new Uri(appBaseUri, resourceLocater);
            var parserContext = new ParserContext
            {
                BaseUri = uri
            };

            var loadBamlInfo = typeof(XamlReader).GetMethod("LoadBaml", BindingFlags.NonPublic | BindingFlags.Static);
            ArgumentNullException.ThrowIfNull(loadBamlInfo);
            
            loadBamlInfo.Invoke(null, new object[] { stream, parserContext, userControl, true });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Cannot load view from uri with exception: {ex.Message}");
        }
    }
}
