namespace Shadcn.Maui.Core;

public static class VisualElementExtensions
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

    public static (double x, double y) PositionRelativeToPage(this VisualElement parent)
    {
        double x = 0, y = 0;

        Element current = parent;
        while (current != null)
        {
            if (current is Page)
            {
                break;
            }

            if (current is VisualElement ve)
            {
                x += ve.X;
                y += ve.Y;
            }

            current = current.Parent;
        }

        return (x, y);
    }
}
