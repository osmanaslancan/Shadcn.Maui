using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
namespace Shadcn.Maui.Controls;

public class SBorder : ContentView
{
    /// <summary>Bindable property for <see cref="Fill"/>.</summary>
    public static readonly BindableProperty FillProperty =
            BindableProperty.Create(nameof(Fill), typeof(Brush), typeof(SBorder), null);

    /// <summary>Bindable property for <see cref="Stroke"/>.</summary>
    public static readonly BindableProperty StrokeProperty =
        BindableProperty.Create(nameof(Stroke), typeof(Brush), typeof(SBorder), null);

    /// <summary>Bindable property for <see cref="StrokeThickness"/>.</summary>
    public static readonly BindableProperty StrokeThicknessProperty =
        BindableProperty.Create(nameof(StrokeThickness), typeof(double), typeof(SBorder), 1.0);

    /// <summary>Bindable property for <see cref="StrokeDashArray"/>.</summary>
    public static readonly BindableProperty StrokeDashArrayProperty =
        BindableProperty.Create(nameof(StrokeDashArray), typeof(DoubleCollection), typeof(SBorder), null,
            defaultValueCreator: bindable => new DoubleCollection());

    /// <summary>Bindable property for <see cref="StrokeDashOffset"/>.</summary>
    public static readonly BindableProperty StrokeDashOffsetProperty =
        BindableProperty.Create(nameof(StrokeDashOffset), typeof(double), typeof(SBorder), 0.0);

    /// <summary>Bindable property for <see cref="StrokeLineCap"/>.</summary>
    public static readonly BindableProperty StrokeLineCapProperty =
        BindableProperty.Create(nameof(StrokeLineCap), typeof(PenLineCap), typeof(SBorder), PenLineCap.Flat);

    /// <summary>Bindable property for <see cref="StrokeLineJoin"/>.</summary>
    public static readonly BindableProperty StrokeLineJoinProperty =
        BindableProperty.Create(nameof(StrokeLineJoin), typeof(PenLineJoin), typeof(SBorder), PenLineJoin.Miter);

    /// <summary>Bindable property for <see cref="StrokeMiterLimit"/>.</summary>
    public static readonly BindableProperty StrokeMiterLimitProperty =
        BindableProperty.Create(nameof(StrokeMiterLimit), typeof(double), typeof(SBorder), 10.0);

    public static new readonly BindableProperty BackgroundProperty =
        BindableProperty.Create(nameof(Background), typeof(Brush), typeof(SBorder), Brush.Default);

    public static new readonly BindableProperty PaddingProperty =
        BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(SBorder), Thickness.Zero);

    public static new readonly BindableProperty BackgroundColorProperty =
        BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(SBorder), null,
            propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                var self = (SBorder)bindableObject;
                self.Background = new SolidColorBrush((Color)newValue);
            });
    public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(SBorder), new CornerRadius());


    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

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

    public new Thickness Padding
    {
        get => (Thickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
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

    private void BindToBackground(RoundRectangle background, VisualElement realBorder)
    {
        background.Bind(RoundRectangle.HeightRequestProperty, nameof(VisualElement.Height), source: realBorder)
            .Bind(RoundRectangle.WidthRequestProperty, nameof(VisualElement.Width), source: realBorder)
            .Bind(RoundRectangle.CornerRadiusProperty, nameof(CornerRadius), source: this)
            .Bind(RoundRectangle.FillProperty, nameof(Background), source: this)
            .Bind(RoundRectangle.StrokeProperty, nameof(Background), source: this);

    }

    private void BindToBorder(RoundRectangle border, VisualElement realBorder)
    {
        border.Bind(RoundRectangle.HeightRequestProperty, nameof(VisualElement.Height), source: realBorder)
            .Bind(RoundRectangle.WidthRequestProperty, nameof(VisualElement.Width), source: realBorder)
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
                        Background = Colors.Transparent,
                        Stroke = null,
                        StrokeShape = new RoundRectangle()
                            .Bind(RoundRectangle.CornerRadiusProperty, nameof(CornerRadius), source: this),
                        Content = new ContentPresenter(),
                    }
                    .Bind(Border.PaddingProperty, nameof(Padding), source: this)
                    .Assign(out Border clipper)
                }
            }.Assign(out VisualElement realBorder);

            BindToBackground(background, realBorder);
            BindToBorder(border, realBorder);
            clipper
             .Bind(Border.HeightRequestProperty, nameof(Height), source: realBorder)
             .Bind(Border.WidthRequestProperty, nameof(Width), source: realBorder);

            return result;
        })
        {

        };
        StyleClass = ["Shadcn-SBorder"];

    }
}
