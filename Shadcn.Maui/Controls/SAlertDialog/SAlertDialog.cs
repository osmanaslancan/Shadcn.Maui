using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SAlertDialog : ContentView
{
    public static readonly BindableProperty TriggerViewProperty =
        BindableProperty.Create(nameof(TriggerView), typeof(View), typeof(SAlertDialog), propertyChanging: OnTriggerViewChanging, propertyChanged: OnTriggerViewChanged);

    public View TriggerView
    {
        get => (View)GetValue(TriggerViewProperty);
        set => SetValue(TriggerViewProperty, value);
    }

    private readonly TapGestureRecognizer _tapGestureRecognizer;

    public static new readonly BindableProperty ContentProperty =
        BindableProperty.Create(nameof(Content), typeof(Popup), typeof(SAlertDialog));

    public new Popup Content
    {
        get => (Popup)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public SAlertDialog()
    {
        _tapGestureRecognizer = new TapGestureRecognizer() { Command = new AsyncRelayCommand(ShowDialog) };
        StyleClass = ["Shadcn-SAlertDialog"];
        ControlTemplate = new ControlTemplate(() =>
            new ContentView().Bind(ContentView.ContentProperty, nameof(TriggerView), source: RelativeBindingSource.TemplatedParent)
        );
    }

    public async Task ShowDialog()
    {
        var page = this.FindParentOfType<Page>() ?? throw new Exception("SAlertDialog needs a parent page to show its dialog");

        if (Content is not null)
        {
            Content.Color = Colors.Transparent;
            await page.ShowPopupAsync(Content);
        }

    }

    private static void OnTriggerViewChanging(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SAlertDialog)bindable;

        if (oldValue is not null)
        {
            if (oldValue is View oldView)
            {
                oldView.GestureRecognizers.Remove(self._tapGestureRecognizer);
            }
        }
    }

    private static void OnTriggerViewChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SAlertDialog)bindable;
        if (newValue is not null)
        {
            if (newValue is View newView)
            {
                newView.GestureRecognizers.Add(self._tapGestureRecognizer);
            }
        }
    }
}
