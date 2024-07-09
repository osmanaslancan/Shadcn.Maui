
using Microsoft.UI.Input;
using System.Reflection;

namespace Shadcn.Maui.Behaviors;

public partial class CursorPointerBehavior
{
    private void ChangePointer(View view, InputSystemCursorShape shape)
    {
        if (view.Handler?.PlatformView is Microsoft.UI.Xaml.UIElement uiElement)
        {
            var cursorProperty = typeof(Microsoft.UI.Xaml.UIElement).GetProperty("ProtectedCursor", BindingFlags.Instance | BindingFlags.NonPublic);

            cursorProperty?.SetValue(uiElement, InputSystemCursor.Create(shape));
        }   
    }

    partial void CursorArrow(View view)
    {
        ChangePointer(view, InputSystemCursorShape.Arrow);
    }

    partial void CursorPointer(View view)
    {
        ChangePointer(view, InputSystemCursorShape.Hand);
    }
}
