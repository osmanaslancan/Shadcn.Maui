using Microsoft.UI.Xaml.Controls;
using WinRT;

namespace Shadcn.Maui.Behaviors;

public partial class EntryExtensionsBehavior : PlatformBehavior<Entry, TextBox>
{
    private Microsoft.UI.Xaml.Style? _oldStyle;

    protected override void OnAttachedTo(Entry bindable, TextBox platformView)
    {
        _oldStyle = platformView.Style;
        platformView.Style = new Microsoft.UI.Xaml.Style(typeof(TextBox))
        {
            Setters =
            {
                new Microsoft.UI.Xaml.Setter(TextBox.BorderThicknessProperty, new Microsoft.UI.Xaml.Thickness(0)),
                new Microsoft.UI.Xaml.Setter(TextBox.CornerRadiusProperty, new Microsoft.UI.Xaml.CornerRadius(4))
            }
        };
    }

    protected override void OnDetachedFrom(Entry bindable, TextBox platformView)
    {
        platformView.Style = _oldStyle ?? Microsoft.UI.Xaml.Application.Current.Resources[typeof(TextBox)] as Microsoft.UI.Xaml.Style;
    }
}