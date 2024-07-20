namespace Shadcn.Maui.Controls;

public class SCollapsible : ContentView
{
    public static readonly BindableProperty IsCollapsedProperty = BindableProperty.Create(nameof(IsCollapsed), typeof(bool), typeof(SCollapsible), true);

    public bool IsCollapsed
    {
        get { return (bool)GetValue(IsCollapsedProperty); }
        set { SetValue(IsCollapsedProperty, value); }
    }
}
