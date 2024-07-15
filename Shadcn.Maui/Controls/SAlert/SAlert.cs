using Shadcn.Maui.Core;
using System.ComponentModel;

namespace Shadcn.Maui.Controls;

public class SAlert : ContentView
{
    public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(View), typeof(SAlert), propertyChanged: OnIconPropertyChanged);
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(View), typeof(SAlert), propertyChanged: OnTitlePropertyChanged);
    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(Description), typeof(View), typeof(SAlert), propertyChanged: OnDescriptionPropertyChanged);

    public View Icon
    {
        get { return (View)GetValue(IconProperty); }
        set { SetValue(IconProperty, value); }
    }

    [TypeConverter(typeof(StringToLabelTypeConverter))]
    public View Title
    {
        get { return (View)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    [TypeConverter(typeof(StringToLabelTypeConverter))]
    public View Description
    {
        get { return (View)GetValue(DescriptionProperty); }
        set { SetValue(DescriptionProperty, value); }
    }

    private static void AddClass(View view, string className)
    {
        if (view.StyleClass is null)
        {
            view.StyleClass = ["SAlert-Title"];
        }
        else
        {
            view.StyleClass = [.. view.StyleClass, "SAlert-Title"];
        }
    }

    private static void OnIconPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is null)
            return;

        var icon = (View)newValue;

        AddClass(icon, "SAlert-Icon");
    }

    private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is null)
            return;

        var title = (View)newValue;

        AddClass(title, "SAlert-Title");
    }

    private static void OnDescriptionPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (newValue is null)
            return;

        var description = (View)newValue;

        AddClass(description, "SAlert-Description");
    }
}
