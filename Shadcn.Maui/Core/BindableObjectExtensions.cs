using CommunityToolkit.Maui.Markup;

namespace Shadcn.Maui;

public static class BindableObjectExtensions
{
    public static T SetBindableValue<T>(this T bindableObject, BindableProperty property, object? value)
        where T : BindableObject
    {
        bindableObject.SetValue(property, value);
        return bindableObject;
    }

    public static T ResourceAppThemeBinding<T>(this T bindableObject, BindableProperty property, ResourceDictionary resource, string color, string darkPrefix = "Dark")
        where T : BindableObject
    {
        return bindableObject.AppThemeBinding(property, resource[color], resource[darkPrefix + color]);
    }
}