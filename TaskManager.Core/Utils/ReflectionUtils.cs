using System.Reflection;

namespace TaskManager.Core.Utils;

public static class ReflectionUtils
{
    private const BindingFlags AllFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

    public static TValue GetFieldValue<TValue>(object obj, string fieldName)
    {
        return GetFieldValue<TValue>(obj.GetType(), obj, fieldName);
    }

    public static TValue GetFieldValue<TValue>(Type type, object? obj, string fieldName)
    {
        var value = type.GetField(fieldName, AllFlags)?.GetValue(obj);
        if(value is null)
        {
            throw new NullReferenceException($"Cannot find {fieldName} field.");
        }
        return (TValue)value;
    }

    public static void SetFieldValue<TValue>(object obj, string fieldName, TValue value)
    {
        SetFieldValue(obj.GetType(), obj, fieldName, value);
    }

    public static void SetFieldValue<TValue>(Type type, object obj, string fieldName, TValue value)
    {
        var fieldInfo = type.GetField(fieldName, AllFlags)
            ?? throw new ArgumentNullException($"Cannot find {fieldName} field.");

        fieldInfo.SetValue(obj, value);
    }
}
