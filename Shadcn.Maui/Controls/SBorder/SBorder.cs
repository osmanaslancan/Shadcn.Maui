using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
namespace Shadcn.Maui.Controls;

public class SBorder : ContentView
{
    /// <summary>Bindable property for <see cref="Fill"/>.</summary>
    public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(Shape), null);

    /// <summary>Bindable property for <see cref="Stroke"/>.</summary>
    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(Shape), null);

    /// <summary>Bindable property for <see cref="StrokeThickness"/>.</summary>
    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(Shape), 1.0);

    /// <summary>Bindable property for <see cref="StrokeDashArray"/>.</summary>
    public static readonly BindableProperty StrokeDashArrayProperty =
        BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(Shape), null,
            defaultValueCreator: bindable => new DoubleCollection());

    /// <summary>Bindable property for <see cref="StrokeDashOffset"/>.</summary>
    public static readonly BindableProperty StrokeDashOffsetProperty =
        BindableProperty.Create(nameof(StrokeDashOffset), typeof(double), typeof(Shape), 0.0);

    /// <summary>Bindable property for <see cref="StrokeLineCap"/>.</summary>
    public static readonly BindableProperty StrokeLineCapProperty =
        BindableProperty.Create(nameof(StrokeLineCap), typeof(PenLineCap), typeof(Shape), PenLineCap.Flat);

    /// <summary>Bindable property for <see cref="StrokeLineJoin"/>.</summary>
    public static readonly BindableProperty StrokeLineJoinProperty =
        BindableProperty.Create(nameof(StrokeLineJoin), typeof(PenLineJoin), typeof(Shape), PenLineJoin.Miter);

    /// <summary>Bindable property for <see cref="StrokeMiterLimit"/>.</summary>
    public static readonly BindableProperty StrokeMiterLimitProperty =
        BindableProperty.Create(nameof(StrokeMiterLimit), typeof(double), typeof(Shape), 10.0);

    public static new readonly BindableProperty BackgroundProperty =
        BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SBorder), Brush.Default);

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static new readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SBorder), null,
            propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                var self = (SBorder)bindableObject;
                self.Background = new SolidColorBrush((Color)newValue);
            });
    public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SBorder), new CornerRadius());

    public new Color BackgroundColor
    {
        get => (Color)GetValue(BackgroundColorProperty);
        set => SetValue(BackgroundColorProperty, value);
    }

    /// <summary>Bindable property for <see cref="StrokeThickness"/>.</summary>
    public double StrokeThickness
    {
        get => (double)GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    /// <summary>Bindable property for <see cref="StrokeDashArray"/>.</summary>
    public DoubleCollection StrokeDashArray
    {
        get => (DoubleCollection)GetValue(StrokeDashArrayProperty);
        set => SetValue(StrokeDashArrayProperty, value);
    }

    /// <summary>Bindable property for <see cref="StrokeDashOffset"/>.</summary>
    public double StrokeDashOffset
    {
        get => (double)GetValue(StrokeDashOffsetProperty);
        set => SetValue(StrokeDashOffsetProperty, value);
    }

    /// <summary>Bindable property for <see cref="StrokeLineCap"/>.</summary>
    public PenLineCap StrokeLineCap
    {
        get => (PenLineCap)GetValue(StrokeLineCapProperty);
        set => SetValue(StrokeLineCapProperty, value);
    }

    /// <summary>Bindable property for <see cref="StrokeLineJoin"/>.</summary>
    public PenLineJoin StrokeLineJoin
    {
        get => (PenLineJoin)GetValue(StrokeLineJoinProperty);
        set => SetValue(StrokeLineJoinProperty, value);
    }

    /// <summary>Bindable property for <see cref="StrokeMiterLimit"/>.</summary>
    public double StrokeMiterLimit
    {
        get => (double)GetValue(StrokeMiterLimitProperty);
        set => SetValue(StrokeMiterLimitProperty, value);
    }

    public Brush Fill
    {
        get => (Brush)GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public Brush Stroke
    {
        get => (Brush)GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    private object GetColor(string color)
    {
        return Application.Current!.Resources[color];
    }

    private void BindToBackground(RoundRectangle background)
    {
        background.Bind(RoundRectangle.HeightRequestProperty, "Content.Height", source: this)
            .Bind(RoundRectangle.WidthRequestProperty, "Content.Width", source: this)
            .Bind(RoundRectangle.CornerRadiusProperty, nameof(CornerRadius), source: this)
            .Bind(RoundRectangle.FillProperty, nameof(Background), source: this)
            .Bind(RoundRectangle.StrokeProperty, nameof(Background), source: this);

    }

    private void BindToBorder(RoundRectangle border)
    {
        border.Bind(RoundRectangle.HeightRequestProperty, "Content.Height", source: this)
            .Bind(RoundRectangle.WidthRequestProperty, "Content.Width", source: this)
            .Bind(RoundRectangle.CornerRadiusProperty, nameof(CornerRadius), source: this)
            .Bind(RoundRectangle.StrokeProperty, nameof(Stroke), source: this)
            .Bind(RoundRectangle.StrokeThicknessProperty, nameof(StrokeThickness), source: this)
            .Bind(RoundRectangle.StrokeDashArrayProperty, nameof(StrokeDashArray), source: this)
            .Bind(RoundRectangle.StrokeDashOffsetProperty, nameof(StrokeDashOffset), source: this)
            .Bind(RoundRectangle.StrokeLineCapProperty, nameof(StrokeLineCap), source: this)
            .Bind(RoundRectangle.StrokeLineJoinProperty, nameof(StrokeLineJoin), source: this)
            .Bind(RoundRectangle.StrokeMiterLimitProperty, nameof(StrokeMiterLimit), source: this);
    }

    public SBorder()
    {
        ControlTemplate = new ControlTemplate(() =>
        {
            var result = new AbsoluteLayout
            {
                Children =
                {
                    new RoundRectangle()
                    {
                    }.Assign(out RoundRectangle background),
                    new RoundRectangle()
                    {
                    }.Assign(out RoundRectangle border),
                    new Border
                    {
                        VerticalOptions = LayoutOptions.Start,
                        Background = Colors.Transparent,
                        Stroke = null,
                        StrokeShape = new RoundRectangle()
                            .Bind(RoundRectangle.CornerRadiusProperty, nameof(CornerRadius), source: this),
                        Content = new ContentPresenter(),
                    },
                }
            };

            BindToBackground(background);
            BindToBorder(border);

            return result;
        });


    }
}
