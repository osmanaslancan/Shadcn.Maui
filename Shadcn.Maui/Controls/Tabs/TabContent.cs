namespace Shadcn.Maui.Controls;

public class TabContent : ContentView
{
    public static readonly BindableProperty TabNameProperty = BindableProperty.Create(nameof(TabName), typeof(string), typeof(TabContent), null);

    public string? TabName
    {
        get { return (string?)GetValue(TabNameProperty); }
        set { SetValue(TabNameProperty, value); }
    }
}
