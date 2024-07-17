
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Shadcn.Maui.Behaviors;
using Shadcn.Maui.Controls;
using System.Diagnostics;

namespace Shadcn.Maui.Resources;

public partial class StyleResource : ResourceDictionary
{

    public StyleResource()
    {
        InitializeComponent();
        RegisterSCardStyles();
    }

    private Color GetColor(string color)
    {
        return (Color)this[color];
    }

    private Style<T> NewStyle<T>(string className, bool applyToDerivedTypes = false, bool canCascade = true)
        where T : BindableObject
    {
        var style = new Style<T>();
        style.MauiStyle.Class = "Shadcn-" + className;
        style.ApplyToDerivedTypes(applyToDerivedTypes);
        style.CanCascade(canCascade);
        return style;
    }

    private Style<T> RegisterSelectorStyle<T>(string selector)
        where T : BindableObject
    {
        Style implicitStyle;

        if (!TryGetValue(typeof(T).FullName, out var value))
        {
            implicitStyle = new Style<T>();
            Add(implicitStyle);
        }
        else
        {
            implicitStyle = (Style)value;
        }

        var innerStyle = new Style<T>();

        implicitStyle.Behaviors.Add(new SmartStyleBehavior() { Selector = selector, Style = innerStyle.MauiStyle });

        return innerStyle;
    }

    private void RegisterStyle(Style style)
    {
        // To prevent any kind of implicit style
        Debug.Assert(style.Class is not null);

        Add(style);
        Add(style.Class, style);
    }

    private void RegisterSCardStyles()
    {
        RegisterStyle(
            NewStyle<SCard>("SCard")
            .Add(SCard.ControlTemplateProperty, new ControlTemplate(() =>
            {
                var innerContent = new Border
                {
                    StyleClass = ["Shadcn-SCard-Border"],
                    Content = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.Start,
                    }.Bind(BindableLayout.ItemsSourceProperty, nameof(SCard.Children), source: RelativeBindingSource.TemplatedParent)
                                    .SetBindableValue(BindableLayout.ItemTemplateProperty, new DataTemplate(() => new ContentPresenter().Bind(ContentPresenter.ContentProperty)))
                };

                return new AbsoluteLayout
                {
                    Children =
                    {
                        new RoundRectangle()
                        {
                            CornerRadius = new CornerRadius(6),
                            StrokeThickness = 1,
                        }.AppThemeBinding(RoundRectangle.FillProperty, GetColor("Card"), GetColor("DarkCard"))
                        .AppThemeBinding(RoundRectangle.StrokeProperty, GetColor("Card"), GetColor("DarkCard"))
                        .Bind(RoundRectangle.HeightRequestProperty, "Height", source: innerContent)
                        .Bind(RoundRectangle.WidthRequestProperty, "Width", source: innerContent),
                        new RoundRectangle()
                        {
                            CornerRadius = new CornerRadius(6),
                            StrokeThickness = 1,
                        }
                        .Bind(RoundRectangle.HeightRequestProperty, "Height", source: innerContent)
                        .Bind(RoundRectangle.WidthRequestProperty, "Width", source: innerContent),
                        innerContent,
                    }
                };
            })));

        RegisterStyle(NewStyle<Border>("SCard-Border")
            .Add(
                (Border.BackgroundColorProperty, Colors.Transparent),
                (Border.StrokeProperty, Colors.Transparent),
                (Border.StrokeShapeProperty, new RoundRectangle() { CornerRadius = new CornerRadius(6) }),
                (Border.ShadowProperty, new Shadow() { Offset = new Point(0, 0), Brush = new SolidColorBrush(Colors.Black), Opacity = 0.1f, Radius = 1f })));

        RegisterStyle(NewStyle<StackLayout>("SCard-StackLayout")
            .Add(StackLayout.OrientationProperty, StackOrientation.Vertical));

        RegisterStyle(NewStyle<SCardHeader>("SCardHeader")
            .Add(
                (SCardHeader.DirectionProperty, FlexDirection.Column),
                (SCardHeader.MarginProperty, new Thickness(24))));

        RegisterStyle(NewStyle<SCardFooter>("SCardFooter")
            .Add(
                (SCardFooter.MarginProperty, new Thickness(24, 0, 24, 24))));

        RegisterSelectorStyle<Label>("SCardTitle Label")
            .Add(
                (Label.FontSizeProperty, 14),
                (Label.LineHeightProperty, 1),
                (Label.CharacterSpacingProperty, -0.4),
                (Label.FontFamilyProperty, "GeistSemiBold"))
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("Primary"), GetColor("DarkPrimary"));

        RegisterSelectorStyle<Label>("SCardDescription Label")
            .Add(
                (Label.FontSizeProperty, 14))
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("MutedForeground"), GetColor("DarkMutedForeground"));
    }
}