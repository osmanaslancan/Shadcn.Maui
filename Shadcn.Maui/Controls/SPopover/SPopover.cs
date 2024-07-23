using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;

using Microsoft.Maui.Controls;
using Shadcn.Maui.Core;
using System.Diagnostics;

namespace Shadcn.Maui.Controls;

public class SPopover : ContentView
{
    public static readonly BindableProperty TriggerViewProperty = BindableProperty.Create(nameof(TriggerView), typeof(View), typeof(SPopover), propertyChanged: OnTriggerViewPropertyChanged);

    public View TriggerView
    {
        get { return (View)GetValue(TriggerViewProperty); }
        set { SetValue(TriggerViewProperty, value); }
    }

    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(SPopover), false, propertyChanged: IsOpenChanged, defaultBindingMode: BindingMode.TwoWay);

    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    private Popup _popup;
    private ContentView _contentView;

    private static void IsOpenChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        var self = (SPopover)bindableObject;
        var value = (bool)newValue;

        if (value)
        {
            if (self.IsLoaded)
                self.ShowPopup();
            else
            {
                EventHandler? eventHandler = null;
                eventHandler = (s, e) =>
                {
                    self.ShowPopup();
                    self.Loaded -= eventHandler;
                };
                self.Loaded += eventHandler;
            }
        }
        else
        {
            self.ClosePopup();
        }
    }

    public SPopover()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            return new ContentView()
                .TapGesture(() => IsOpen = true)
                .Bind(ContentView.ContentProperty, nameof(TriggerView), source: this)
                .Bind(ContentView.BindingContextProperty, nameof(BindingContext), source: this);
        });

        _popup = new Popup()
        {
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
                        .Assign(out _contentView)

                    }
                }.TapGesture(AnimatedClose)
            }
        };
        
        _popup.Opened += (s, e) =>
        {
            if (!IsOpen)
            {
                _popup.Close();
                return;
            }

            _popup.Window.RemoveOverlay(_popup.Window.Overlays.First());
            _contentView!.Scale = 0;
            _contentView.Opacity = 0;
            _contentView.Anchor(0.5, 0.1);
            _contentView.Animate("ScaleWithOpacity", new Animation((step) =>
            {
                _contentView.Opacity = step;
                _contentView.Scale = step;
            }, start: 0.8, easing: Easing.CubicOut), length: 150);
        };

        _popup.Closed += (s, e) =>
        {
            IsOpen = false;
        };

        Loaded += (sender, e) =>
        {
            var page = this.FindParentOfType<Page>() ?? throw new InvalidOperationException("Cant find a parent page to show popover");

            _popup.Size = new Size(page.Width, page.Height);
            page.SizeChanged += updateSize;

            void updateSize(object? sender, EventArgs args)
            {
                _popup.Close();
            }
        };
    }

    private void ClosePopup()
    {
        AnimatedClose();
    }

    private void AnimatedClose()
    {
        if (_contentView is null || _popup is null || _contentView.AnimationIsRunning("ScaleWithOpacity"))
            return;

        _contentView.Animate("ScaleWithOpacity", new Animation((step) =>
        {
            _contentView.Opacity = 1 - (step - 0.8);
            _contentView.Scale = 1 - (step - 0.8);
            if (step == 1)
                _contentView.Opacity = 0;

        }, start: 0.8, easing: Easing.CubicIn), length: 150, finished: (_, _) =>
        {
            _popup.Close();
        });
    }

    private void ShowPopup()
    {
        if (Content is null)
            return;

        var page = this.FindParentOfType<Page>() ?? throw new InvalidOperationException("Cant find a parent page to show popover");

        var (x, y) = this.PositionRelativeToPage();

        _contentView.LayoutBounds(x, y + 90);

        page.ShowPopup(_popup);
        page.ShowPopup(_popup);
    }

    private static void OnTriggerViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        const string styleClass = "Shadcn-SPopoverTriggerView";

        if (oldValue is View oldView && oldView.StyleClass.Contains(styleClass))
        {
            oldView.StyleClass.Remove(styleClass);
            oldView.StyleClass = [.. oldView.StyleClass];
        }

        if (newValue is View newView && !newView.StyleClass.Contains(styleClass))
        {
            newView.StyleClass.Add(styleClass);
            newView.StyleClass = [.. newView.StyleClass];
        }
    }
}
