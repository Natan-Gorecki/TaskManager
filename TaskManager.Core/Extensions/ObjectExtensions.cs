using System.Reflection;

namespace TaskManager.Core.Extensions;

internal static class ObjectExtensions
{
    public static T CopyFrom<T>(this T destination, T source)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(destination);
        ArgumentNullException.ThrowIfNull(source);

        foreach(PropertyInfo sourceProperty in source.GetType().GetProperties().Where(p => p.CanWrite))
        {
            var setInfo = sourceProperty.GetSetMethod(nonPublic: true);

            if(setInfo is null)
            {
                continue;
            }

            if(setInfo.IsPublic || setInfo.IsAssembly || setInfo.IsFamilyOrAssembly)
            {
                PropertyInfo? destinationProperty = destination.GetType().GetProperty(sourceProperty.Name);
                destinationProperty?.SetValue(destination, sourceProperty.GetValue(source));
            }
        }

        return destination;
    }
}
