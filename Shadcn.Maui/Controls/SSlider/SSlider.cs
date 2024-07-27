using CommunityToolkit.Maui.Markup;
using System.Globalization;

namespace Shadcn.Maui.Controls;

public class SSlider : TemplatedView
{
    public static readonly BindableProperty MinValueProperty = BindableProperty.Create(nameof(MinValue), typeof(double), typeof(SSlider), 0d);
    public static readonly BindableProperty MaxValueProperty = BindableProperty.Create(nameof(MaxValue), typeof(double), typeof(SSlider), 100d);
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(SSlider), 0d);
    public static readonly BindableProperty StepProperty = BindableProperty.Create(nameof(Step), typeof(double), typeof(SSlider), 1d);

    public double MinValue
    {
        get => (double)GetValue(MinValueProperty);
        set => SetValue(MinValueProperty, value);
    }

    public double MaxValue
    {
        get => (double)GetValue(MaxValueProperty);
        set => SetValue(MaxValueProperty, value);
    }

    public double Value
    {
        get => (double)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public double Step
    {
        get => (double)GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public SSlider()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            var gesture = new PointerGestureRecognizer();
            gesture.PointerPressed += OnTrackPressed;
            var panGesture = new PanGestureRecognizer();
            panGesture.PanUpdated += OnPanUpdated;

            var result = new AbsoluteLayout()
            {
                GestureRecognizers =
                {
                    gesture,
                    panGesture
                },
                Children =
                {
                    new SBorder()
                    {
                        StyleClass = ["Shadcn-SSlider-Track"],
                        Content = new AbsoluteLayout()
                        {
                            new SBorder()
                            {
                                StyleClass = ["Shadcn-SSlider-Range"]
                            }
                            .LayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.SizeProportional)
                            .Bind(AbsoluteLayout.LayoutBoundsProperty, new Binding("Value", source: this), new Binding("MinValue", source: this), new Binding("MaxValue", source: this),
                                convert: ((double value, double min, double max) binds) => new Rect(0, 0, Math.Min(binds.value / (binds.max - binds.min), 1), 1))
                            .Bind(SBorder.IsVisibleProperty, new Binding("Value", source: this), new Binding("MinValue", source: this), convert: ((double value, double min) binds) => binds.value > binds.min),
                        }
                    }
                    .LayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.SizeProportional)
                    .LayoutBounds(0, 0, 1, 1),
                     new SBorder()
                     {
                        StyleClass = ["Shadcn-SSlider-Thumb"],
                     }.LayoutFlags(Microsoft.Maui.Layouts.AbsoluteLayoutFlags.YProportional)
                     .Bind(AbsoluteLayout.LayoutBoundsProperty, new Binding("Value", source: this), new Binding("MinValue", source: this), new Binding("MaxValue", source: this), new Binding("Width", source: this),
                                convert: ((double value, double min, double max, double width) binds) => new Rect(Math.Min(binds.value / (binds.max - binds.min), 1) * binds.width - 12.5, 0.5, -1, -1))
                }
            };

            return result;
        });
    }

    private void OnTrackPressed(object? sender, PointerEventArgs args)
    {
        var position = args.GetPosition(this);

        var percentageValue = position!.Value.X / Width;

        var stepValue = Math.Round((MinValue + (MaxValue - MinValue) * percentageValue) / Step) * Step;
        Value = Math.Clamp(stepValue, MinValue, MaxValue);
    }

    private double? _panInitialValue;

    private void OnPanUpdated(object? sender, PanUpdatedEventArgs args)
    {
        if (args.StatusType == GestureStatus.Completed)
            return;

        if (args.StatusType == GestureStatus.Started)
            _panInitialValue = Value / (MaxValue - MinValue);

        var position = args.TotalX;

        var percentageValue = Math.Clamp(position / Width + _panInitialValue!.Value, 0, 1);

        var stepValue = Math.Round((MinValue + (MaxValue - MinValue) * percentageValue) / Step) * Step;
        Value = Math.Clamp(stepValue, MinValue, MaxValue);
    }
}
