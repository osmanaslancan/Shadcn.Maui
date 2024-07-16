namespace Shadcn.Maui;

public static class BindableObjectExtensions 
{
    public static T SetBindableValue<T>(this T bindableObject, BindableProperty property, object? value) 
        where T : BindableObject
    {
        bindableObject.SetValue(property, value);
        return bindableObject;
    }
}