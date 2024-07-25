using CommunityToolkit.Maui.Markup;
using CommunityToolkit.Maui.Views;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SContextMenu : ContentView
{
    public static readonly BindableProperty TriggerViewProperty = BindableProperty.Create(nameof(TriggerView), typeof(View), typeof(SPopover));

    public View TriggerView
    {
        get { return (View)GetValue(TriggerViewProperty); }
        set { SetValue(TriggerViewProperty, value); }
    }

    private Popup _popup;
    private ContentView _contentView;
    private readonly TapGestureRecognizer gestureRecognizer;

    public SContextMenu()
    {
        gestureRecognizer = new TapGestureRecognizer()
        {
            Buttons = ButtonsMask.Secondary,
        };
        gestureRecognizer.Tapped += ShowPopup;

        ControlTemplate = new ControlTemplate(() =>
        {
            return new ContentView()
                .Invoke(x =>
                {
                    x.GestureRecognizers.Add(gestureRecognizer);
                })
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
            _popup.Window.RemoveOverlay(_popup.Window.Overlays.First());
            _contentView!.Scale = 0;
            _contentView.Opacity = 0;
            _contentView.Anchor(0.1, 0.5);
            _contentView.Animate("ScaleWithOpacity", new Animation((step) =>
            {
                _contentView.Opacity = step;
                _contentView.Scale = step;
            }, start: 0.8, easing: Easing.CubicOut), length: 150);
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


    private void ShowPopup(object? sender, TappedEventArgs e)
    {
        if (Content is null)
            return;

        var page = this.FindParentOfType<Page>() ?? throw new InvalidOperationException("Cant find a parent page to show popover");

        var (x, y) = this.PositionRelativeToPage();

        var offset = e.GetPosition(TriggerView);

        if (offset is not null)
        {
            x += offset.Value.X;
            y += offset.Value.Y;
        }

        _contentView.LayoutBounds(x, y + 45);

        page.ShowPopup(_popup);
        page.ShowPopup(_popup);
    }
}
