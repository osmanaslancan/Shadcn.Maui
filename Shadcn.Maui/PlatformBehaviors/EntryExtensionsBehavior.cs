using Shadcn.Maui.Core;

namespace Shadcn.Maui.Behaviors;

public partial class EntryExtensionsBehavior
{
    public static readonly BindableProperty HasUnderLineProperty =
       BindableProperty.CreateAttached("HasUnderLine", typeof(bool), typeof(EntryExtensionsBehavior), true, propertyChanged: OnAttachBehaviorChanged);

    public static bool GetHasUnderLine(BindableObject view)
    {
        return (bool)view.GetValue(HasUnderLineProperty);
    }

    public static void SetHasUnderLine(BindableObject view, bool value)
    {
        view.SetValue(HasUnderLineProperty, value);
    }

    static void OnAttachBehaviorChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        BindableObjectHelpers.ToggleBehavior<EntryExtensionsBehavior, Entry>(bindableObject, !(bool)newValue);
    }
}