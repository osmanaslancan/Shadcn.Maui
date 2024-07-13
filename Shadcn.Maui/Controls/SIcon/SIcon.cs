using CommunityToolkit.Maui.Markup;

namespace Shadcn.Maui.Controls;

public class SIcon : Image
{
    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(SIcon));

    public static readonly BindableProperty SizeProperty =
        BindableProperty.Create(nameof(Size), typeof(double), typeof(SIcon));

    public static readonly BindableProperty ColorProperty =
        BindableProperty.Create(nameof(Color), typeof(Color), typeof(SIcon));

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public double Size
    {
        get => (double)GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public Color Color
    {
        get => (Color)GetValue(ColorProperty);
        set => SetValue(ColorProperty, value);
    }

    public SIcon()
    {
        Source = new FontImageSource()
        {
            FontFamily = "Lucide",
        }
        .Bind(FontImageSource.GlyphProperty, nameof(Icon), source: this)
        .Bind(FontImageSource.SizeProperty, nameof(Size), source: this)
        .Bind(FontImageSource.ColorProperty, nameof(Color), source: this);
    }
}
