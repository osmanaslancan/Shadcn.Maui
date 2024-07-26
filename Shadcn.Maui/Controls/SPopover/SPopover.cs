using CommunityToolkit.Maui.Markup;
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

    public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(nameof(IsOpen), typeof(bool), typeof(SPopover), false, propertyChanged: IsOpenChanged, defaultBindingMode: BindingMode.TwoWay);

    public bool IsOpen
    {
        get { return (bool)GetValue(IsOpenProperty); }
        set { SetValue(IsOpenProperty, value); }
    }

    public static readonly BindableProperty HasAnimationProperty = BindableProperty.Create(nameof(HasAnimation), typeof(bool), typeof(SPopover), true);

    public bool HasAnimation
    {
        get { return (bool)GetValue(HasAnimationProperty); }
        set { SetValue(HasAnimationProperty, value); }
    }

    public static readonly BindableProperty PopoverSideProperty = BindableProperty.Create(nameof(PopoverSide), typeof(SPopoverSide), typeof(SPopover), SPopoverSide.Bottom);

    public SPopoverSide PopoverSide
    {
        get { return (SPopoverSide)GetValue(PopoverSideProperty); }
        set { SetValue(PopoverSideProperty, value); }
    }

    private SPopoverSide? _popoverSideOverride;
    private Point? _boundsOffset;

    private ContentView _popoverView;
    private TapGestureRecognizer _closeGestureRecognizer;
    private SPage? parentSPage;

    protected virtual double Spacing => 5;
    protected virtual string TriggerStyleClass => "Shadcn-SPopoverTriggerView";
    private const string AnimationKey = "ScaleWithOpacity";

    private static void IsOpenChanged(BindableObject bindableObject, object oldValue, object newValue)
    {
        var self = (SPopover)bindableObject;
        var value = (bool)newValue;

        self._popoverView.AbortAnimation(AnimationKey);

        if (value)
        {
            if (self.IsLoaded)
                self.OpenPopup();
            else
            {
                void eventHandler(object? s, EventArgs e)
                {
                    self.OpenPopup();
                    self.Loaded -= eventHandler;
                }

                self.Loaded += eventHandler;
            }
        }
        else
        {
            self.ClosePopup();
        }
    }

    protected virtual GestureRecognizer GetTriggerGestureRecognizer()
    {
        return new TapGestureRecognizer()
        {
            Command = new Command(() => IsOpen = !IsOpen)
        };
    }

    public SPopover()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            var view = new ContentView()
                .Bind(ContentView.ContentProperty, nameof(TriggerView), source: this)
                .Bind(ContentView.BindingContextProperty, nameof(BindingContext), source: this);
            view.GestureRecognizers.Add(GetTriggerGestureRecognizer());

            return view;
        });

        _popoverView = new ContentView()
                        .Bind(ContentView.ContentProperty, nameof(Content), source: this)
                        .Anchor(0.5, 0.1)
                        .Assign(out _popoverView);

        _popoverView.Loaded += (sender, e) =>
        {
            BoundsCheck();
        };

        _closeGestureRecognizer = new TapGestureRecognizer() { Command = new Command(() => IsOpen = false) };

        Loaded += (sender, e) =>
        {
            parentSPage = this.FindParentOfType<SPage>() ?? throw new InvalidOperationException("Cant find a parent SPage to show popover");
            parentSPage.SizeChanged += PageSizeChanged;
        };

        Unloaded += (sender, e) =>
        {
            if (parentSPage is not null)
                parentSPage.SizeChanged -= PageSizeChanged;
        };
    }

    private void PageSizeChanged(object? sender, EventArgs e)
    {
        BoundsCheck();
    }

    private async void ClosePopup()
    {
        if (parentSPage is null)
            throw new ArgumentNullException("Cant find a parent SPage to show popover");

        if (HasAnimation)
            await AnimateClose();

        parentSPage.RemoveAbsoluteView(_popoverView);
        parentSPage.RemoveGestureRecognizer(_closeGestureRecognizer);
    }

    private void BoundsCheck()
    {
        if (!_popoverView.IsLoaded)
            return;

        ArgumentNullException.ThrowIfNull(parentSPage);

        var containerWidth = parentSPage.Width;
        var containerHeight = parentSPage.Height;

        var popoverWidth = _popoverView.Width;
        var popoverHeight = _popoverView.Height;

        var side = PopoverSide;

        var triggerPosition = GetTriggerPosition();
        double y = 0;

        switch (side)
        {
            case SPopoverSide.Bottom:
                y = popoverHeight + triggerPosition.Y + triggerPosition.Height;
                if (y > containerHeight)
                    _popoverSideOverride = SPopoverSide.Top;
                else
                    _popoverSideOverride = null;
                break;
            case SPopoverSide.Top:
                y = triggerPosition.Y - popoverHeight;
                if (y < 0)
                    _popoverSideOverride = SPopoverSide.Bottom;
                else
                    _popoverSideOverride = null;
                break;
            default:
                break;
        }

        if (triggerPosition.X + popoverWidth > containerWidth)
        {
            _boundsOffset = new Point(containerWidth - triggerPosition.X - popoverWidth, 0);
        }
        else if (triggerPosition.X < 0)
        {
            _boundsOffset = new Point(-triggerPosition.X, 0);
        }
        else
        {
            _boundsOffset = null;
        }

        PositionPopover();
    }

    protected virtual Rect GetTriggerPosition()
    {
        var (targetX, targetY) = TriggerView.PositionRelativeToPage();

        return new Rect(targetX, targetY - Spacing, TriggerView.Width, TriggerView.Height + 2 * Spacing);
    }

    private void PopoverSizeChanged(object? sender, EventArgs _)
    {
        BoundsCheck();
    }

    protected virtual void ConfigureAnchor(View view, SPopoverSide side)
    {
        switch (side)
        {
            case SPopoverSide.Bottom:
                _popoverView.Anchor(0.5, 0.1);
                break;
            case SPopoverSide.Top:
                _popoverView.Anchor(0.5, 0.9);
                break;
            default:
                break;
        }
    }

    private void PositionPopover()
    {
        _popoverView.RemoveBinding(AbsoluteLayout.LayoutBoundsProperty);

        var triggerPosition = GetTriggerPosition();

        if (_boundsOffset is not null)
        {
            triggerPosition = triggerPosition.Offset(_boundsOffset.Value);
        }

        var side = _popoverSideOverride ?? PopoverSide;

        ConfigureAnchor(_popoverView, side);

        switch (side)
        {
            case SPopoverSide.Bottom:
                _popoverView.LayoutBounds(triggerPosition.X, triggerPosition.Y + triggerPosition.Height);
                break;
            case SPopoverSide.Top:
                _popoverView.Bind(AbsoluteLayout.LayoutBoundsProperty, "Height", source: _popoverView, convert: (double height) =>
                {
                    if (height < 0)
                        return new Rect(0, 0, -1, -1);

                    return new Rect(triggerPosition.X, triggerPosition.Y - height, -1, -1);
                });
                break;
            default:
                break;
        }
    }

    private void OpenPopup()
    {
        if (parentSPage is null)
            throw new ArgumentNullException("Cant find a parent SPage to show popover");

        PositionPopover();

        parentSPage.AddAbsoluteView(_popoverView);
        parentSPage.AddGestureRecognizer(_closeGestureRecognizer);
        if (HasAnimation)
            AnimateOpen();
    }

    private Task AnimateClose()
    {
        var taskCompletionSource = new TaskCompletionSource();
        
        _popoverView.Animate(AnimationKey, new Animation((step) =>
        {
            _popoverView.Opacity = 1 - (step - 0.8);
            _popoverView.Scale = 1 - (step - 0.8);
            if (step == 1)
                _popoverView.Opacity = 0;

        }, start: 0.8, easing: Easing.CubicIn), length: 150, finished: (_, _) => taskCompletionSource.SetResult());

        return taskCompletionSource.Task;
    }

    private Task AnimateOpen()
    {
        var taskCompletionSource = new TaskCompletionSource();

        _popoverView.Scale = 0;
        _popoverView.Opacity = 0;
        _popoverView.Animate(AnimationKey, new Animation((step) =>
        {
            _popoverView.Opacity = step;
            _popoverView.Scale = step;
        }, start: 0.8, easing: Easing.CubicOut), length: 150, finished: (_, _) => taskCompletionSource.SetResult());

        return taskCompletionSource.Task;
    }
  
    private static void OnTriggerViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var self = (SPopover)bindable;

        if (oldValue is View oldView && oldView.StyleClass.Contains(self.TriggerStyleClass))
        {
            oldView.StyleClass.Remove(self.TriggerStyleClass);
            oldView.StyleClass = [.. oldView.StyleClass];
        }

        if (newValue is View newView && !newView.StyleClass.Contains(self.TriggerStyleClass))
        {
            newView.StyleClass.Add(self.TriggerStyleClass);
            newView.StyleClass = [.. newView.StyleClass];
        }
    }
}
