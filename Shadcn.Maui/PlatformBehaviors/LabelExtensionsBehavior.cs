using Shadcn.Maui.Core;

namespace Shadcn.Maui.Behaviors;

public partial class LabelExtensionsBehavior
{
    public static readonly BindableProperty TextSelectableProperty =
       BindableProperty.CreateAttached("TextSelectable", typeof(bool), typeof(LabelExtensionsBehavior), false, propertyChanged: OnAttachBehaviorChanged);

    public static readonly BindableProperty SelectStartProperty =
      BindableProperty.CreateAttached("SelectStart", typeof(int), typeof(LabelExtensionsBehavior), -1);

    public static readonly BindableProperty SelectEndProperty =
      BindableProperty.CreateAttached("SelectEnd", typeof(int), typeof(LabelExtensionsBehavior), -1);

    public static bool GetTextSelectable(BindableObject view)
    {
        return (bool)view.GetValue(TextSelectableProperty);
    }

    public static void SetTextSelectable(BindableObject view, bool value)
    {
        view.SetValue(TextSelectableProperty, value);
    }

    public static int GetSelectStart(BindableObject view)
    {
        return (int)view.GetValue(SelectStartProperty);
    }

    public static void SetSelectStart(BindableObject view, int value)
    {
        view.SetValue(SelectStartProperty, value);
    }

    public static int GetSelectEnd(BindableObject view)
    {
        return (int)view.GetValue(SelectEndProperty);
    }

    public static void SetSelectEnd(BindableObject view, int value)
    {
        view.SetValue(SelectEndProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        BindableObjectHelpers.ToggleBehavior<LabelExtensionsBehavior, Label>(bindableObject, (bool)newValue);
    }
}
