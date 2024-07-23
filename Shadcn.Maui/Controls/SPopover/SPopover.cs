using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;

using Microsoft.Maui.Controls;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SPopover : ContentView
{
    public static readonly BindableProperty TriggerViewProperty = BindableProperty.Create(nameof(TriggerView), typeof(View), typeof(SPopover), propertyChanged: OnTriggerViewPropertyChanged);

    public View TriggerView
    {
        get { return (View)GetValue(TriggerViewProperty); }
        set { SetValue(TriggerViewProperty, value); }
    }

    private Popup? _popup;

    public SPopover()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            return new ContentView()
                .TapGesture(ShowPopup)
                .Bind(ContentView.ContentProperty, nameof(TriggerView), source: this);
        });
    }

    private void ShowPopup()
    {
        if (Content is null)
            return;

        if (_popup is not null && _popup.Parent is not null)
        {
            _popup!.Close();
            return;
        }

        var page = this.FindParentOfType<Page>() ?? throw new InvalidOperationException("Cant find a parent page to show popover");

        var (x, y) = this.PositionRelativeToPage();

        _popup = new Popup()
        {
            Size = new Size(page.Width, page.Height),
            Color = Colors.Transparent,
            Content = new Grid
            {
                new AbsoluteLayout()
                {
                    BackgroundColor = Colors.Transparent,
                    Children =
                    {
                        new ContentView()
                        .Bind(ContentView.ContentProperty, nameof(Content), source: this)
                        .Assign(out ContentView contentView)
                        .LayoutBounds(x, y + 90),
                    }
                }.TapGesture(AnimateThenClose)
            }
        };

        void AnimateThenClose()
        {
            contentView.Animate("ScaleWithOpacity", new Animation((step) =>
            {
                contentView.Opacity = 1 - (step - 0.8);
                contentView.Scale = 1 - (step - 0.8);
                if (step == 1)
                    contentView.Opacity = 0;

            }, start: 0.8, easing: Easing.CubicIn), length: 150, finished: (_, _) =>
            {
                _popup!.Close();
            });
        }

        _popup.Opened += (s, e) =>
        {
            _popup.Window.RemoveOverlay(_popup.Window.Overlays.First());
            contentView.Scale = 0;
            contentView.Opacity = 0;
            contentView.Anchor(0.5, 0.1);
            contentView.Animate("ScaleWithOpacity", new Animation((step) =>
            {
                contentView.Opacity = step;
                contentView.Scale = step;
            }, start: 0.8, easing: Easing.CubicOut), length: 150);
            
        };

        _popup.Closed += (s, e) =>
        {
            contentView.Content = null;
            _popup = null;
        };

        page.ShowPopup(_popup);
    }

    private static void OnTriggerViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        const string styleClass = "Shadcn-SPopoverTriggerView";

        if (oldValue is View oldView && oldView.StyleClass.Contains(styleClass))
        {
                oldView.StyleClass.Remove(styleClass);
                oldView.StyleClass = [..oldView.StyleClass];
        }

        if (newValue is View newView && !newView.StyleClass.Contains(styleClass))
        {
            newView.StyleClass.Add(styleClass);
            newView.StyleClass = [..newView.StyleClass];
        }
    }
}
