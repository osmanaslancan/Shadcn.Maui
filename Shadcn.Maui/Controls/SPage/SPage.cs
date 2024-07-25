using CommunityToolkit.Maui.Markup;
namespace Shadcn.Maui.Controls;

public class SPage : ContentPage
{
    private AbsoluteLayout absoluteLayout = default!;

    public SPage()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            return new AbsoluteLayout
            {
                Children =
                {
                    new ContentPresenter()
                    .LayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.SizeProportional)
                    .LayoutBounds(0, 0, 1, 1)
                }
            }.Assign(out absoluteLayout);
        });
    }

    public void AddAbsoluteView(View content)
    {
        absoluteLayout.Children.Add(content);
    }

    public void RemoveAbsoluteView(View content)
    {
        absoluteLayout.Children.Remove(content);
    }

    public void AddGestureRecognizer(GestureRecognizer gestureRecognizer)
    {
        absoluteLayout.GestureRecognizers.Add(gestureRecognizer);
    }

    public void RemoveGestureRecognizer(GestureRecognizer gestureRecognizer)
    {
        absoluteLayout.GestureRecognizers.Remove(gestureRecognizer);
    }
}
