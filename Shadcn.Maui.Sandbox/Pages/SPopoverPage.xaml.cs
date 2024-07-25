using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;

namespace Shadcn.Maui.Sandbox.Pages;

public partial class SPopoverPage
{
    public SPopoverPage()
    {
        InitializeComponent();
        var dragGestureRecognizer = new DragGestureRecognizer()
        {
            CanDrag = true,
        };
        popover.GestureRecognizers.Add(dragGestureRecognizer);

        var dropGestureRecognizer = new DropGestureRecognizer()
        {
            AllowDrop = true
        };

        dropGestureRecognizer.Drop += (sender, e) =>
        {
            var position = e.GetPosition(absoluteLayout);
            popover.LayoutFlags(AbsoluteLayoutFlags.None);
            popover.LayoutBounds(position!.Value);
        };

        absoluteLayout.GestureRecognizers.Add(dropGestureRecognizer);
    }
}