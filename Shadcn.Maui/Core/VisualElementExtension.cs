namespace Shadcn.Maui.Core;

public static class VisualElementExtension
{
    public static T? FindParentOfType<T>(this VisualElement parent)
        where T : VisualElement
    {
        var current = parent;
        while (current != null)
        {
            if (current is T typedCurrent)
            {
                return typedCurrent;
            }
            current = current.Parent as VisualElement;
        }

        return default;
    }
}
