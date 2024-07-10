using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using System.Reflection;

namespace Shadcn.Maui.Behaviors;

public partial class CursorPointerBehavior : PlatformBehavior<View, FrameworkElement>
{
    private void ChangePointer(FrameworkElement element, InputSystemCursorShape shape)
    {
        var cursorProperty = typeof(UIElement).GetProperty("ProtectedCursor", BindingFlags.Instance | BindingFlags.NonPublic);

        cursorProperty?.SetValue(element, InputSystemCursor.Create(shape));
    }

    protected override void OnAttachedTo(View bindable, FrameworkElement platformView)
    {
        ChangePointer(platformView, InputSystemCursorShape.Hand);
    }

    protected override void OnDetachedFrom(View bindable, FrameworkElement platformView)
    {
        ChangePointer(platformView, InputSystemCursorShape.Arrow);
    }
}
