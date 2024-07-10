using Shadcn.Maui.Core;

namespace Shadcn.Maui.Behaviors;

public partial class CursorPointerBehavior
{
    public static readonly BindableProperty CursorPointerProperty =
        BindableProperty.CreateAttached("CursorPointer", typeof(bool), typeof(CursorPointerBehavior), false, propertyChanged: OnAttachBehaviorChanged);

    public static bool GetCursorPointer(BindableObject view)
    {
        return (bool)view.GetValue(CursorPointerProperty);
    }

    public static void SetCursorPointer(BindableObject view, bool value)
    {
        view.SetValue(CursorPointerProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        BindableObjectHelpers.ToggleBehavior<CursorPointerBehavior, View>(bindableObject, (bool)newValue);
    }
}
