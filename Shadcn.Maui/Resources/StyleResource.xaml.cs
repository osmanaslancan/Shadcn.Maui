
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Layouts;
using Shadcn.Maui.Behaviors;
using Shadcn.Maui.Controls;
using Shadcn.Maui.Core;
using System.Diagnostics;
using C = Shadcn.Maui.Resources.ShadcnColors;

namespace Shadcn.Maui.Resources;

public partial class StyleResource : ResourceDictionary
{
    public StyleResource()
    {
        InitializeComponent();
        RegisterSEntryStyles();
        RegisterSCardStyles();
        RegisterSAlertDialogStyles();
        RegisterSAvatarStyles();
        RegisterSBadgeStyles();
        RegisterSButtonStyles();
        RegisterSDatePickerStyles();
        RegisterSCheckboxStyles();
        RegisterSBorderStyles();
        RegisterSCommandStyles();
        RegisterSPopoverStyles();
        RegisterSSliderStyles();
        RegisterSToggleStyles();
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

    private Style<T> RegisterSelectorStyle<T>(string selector, int order = 0)
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

        implicitStyle.Behaviors.Add(new SmartStyleBehavior() { Selector = selector, Style = innerStyle.MauiStyle, Order = order });

        return innerStyle;
    }

    private void RegisterStyle(Style style)
    {
        // To prevent any kind of implicit style
        Debug.Assert(style.Class is not null);

        Add(style);
        Add(style.Class, style);
    }

    private void RegisterSEntryStyles()
    {
        RegisterStyle(
            NewStyle<Entry>("SEntry-Entry")
            .Add(EntryExtensionsBehavior.HasUnderLineProperty, false)
            .Add(Entry.WidthProperty, 2000)
            .Add(Entry.BackgroundProperty, Brush.Transparent)
        );

        RegisterStyle(
            NewStyle<SBorder>("SEntry-Ring")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Background)
            .Add(SBorder.BackgroundColorProperty, Colors.Transparent)
            .Add(SBorder.MaximumWidthRequestProperty, 320)
            .Add(SBorder.StrokeProperty, Brush.Transparent)
            .Add(SBorder.CornerRadiusProperty, 5)
            .Add(SBorder.StrokeThicknessProperty, 0)
            .Add(SBorder.PaddingProperty, 1)
        );
        
        RegisterSelectorStyle<SBorder>(".Shadcn-SEntry-Ring > SBorder > Entry:IsFocused")
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Ring)
            .Add(SBorder.BackgroundColorProperty, Colors.Red)
            .Add(SBorder.StrokeThicknessProperty, 2);

        RegisterStyle(
            NewStyle<SBorder>("SEntry-Border")
            .Add(SBorder.MaximumWidthRequestProperty, 320)
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Background)
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Border)
            .Add(SBorder.CornerRadiusProperty, 5)
            .Add(SBorder.StrokeThicknessProperty, 1)
            .Add(SBorder.PaddingProperty, 1)
        );
    }

    private void RegisterSCardStyles()
    {
        RegisterStyle(
            NewStyle<SCard>("SCard")
            .Add(SCard.ControlTemplateProperty, new ControlTemplate(() =>
            {
                return new SBorder()
                {
                    CornerRadius = new CornerRadius(6),
                    Content = new StackLayout
                    {
                        StyleClass = ["Shadcn-SCard-StackLayout"],
                        VerticalOptions = LayoutOptions.Start,
                    }.Bind(BindableLayout.ItemsSourceProperty, nameof(SCard.Children), source: RelativeBindingSource.TemplatedParent)
                    .SetBindableValue(BindableLayout.ItemTemplateProperty, new DataTemplate(() => new ContentPresenter().Bind(ContentPresenter.ContentProperty)))
                }.ResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Card)
                .ResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Border)
                .Bind(SBorder.PaddingProperty, nameof(SCard.Padding), source: RelativeBindingSource.TemplatedParent);
            })));

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
            .AddResourceAppThemeBinding(Label.TextColorProperty, this, C.Primary);

        RegisterSelectorStyle<Label>("SCardDescription Label")
            .Add(
                (Label.FontSizeProperty, 14))
            .AddResourceAppThemeBinding(Label.TextColorProperty, this, C.MutedForeground);
    }

    private void RegisterSAlertDialogStyles()
    {
    }

    private void RegisterSAvatarStyles()
    {
        RegisterStyle(NewStyle<SAvatar>("SAvatar")
            .Add(SAvatar.BorderWidthProperty, 0)
            .Add(SAvatar.FontFamilyProperty, "Geist")
            .AddResourceAppThemeBinding(SAvatar.BackgroundColorProperty, this, C.Muted));
    }

    private void RegisterSBadgeStyles()
    {
        RegisterStyle(NewStyle<SBadge>("SBadge")
            .Add(SBadge.CornerRadiusProperty, new CornerRadius(99999))
            .Add(SBadge.PaddingProperty, new Thickness(10, 2)));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge>Label")
            .Add(Label.FontSizeProperty, 14)
            .Add(Label.FontFamilyProperty, "GeistSemiBold");

        RegisterStyle(NewStyle<SBadge>("SBadge-Primary")
            .AddResourceAppThemeBinding(SBadge.BackgroundColorProperty, this, C.Primary)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBadge.BackgroundColorProperty, this, C.Primary80)));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Primary>Label")
            .AddResourceAppThemeBinding(Label.TextColorProperty, this, C.PrimaryForeground);

        RegisterStyle(NewStyle<SBadge>("SBadge-Secondary")
            .AddResourceAppThemeBinding(SBadge.BackgroundColorProperty, this, C.Secondary)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBadge.BackgroundColorProperty, this, C.Secondary80)));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Secondary>Label")
            .AddResourceAppThemeBinding(Label.TextColorProperty, this, C.SecondaryForeground);

        RegisterStyle(NewStyle<SBadge>("SBadge-Destructive")
            .AddResourceAppThemeBinding(SBadge.BackgroundColorProperty, this, C.Destructive)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBadge.BackgroundColorProperty, this, C.Destructive80)));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Destructive>Label")
            .AddResourceAppThemeBinding(Label.TextColorProperty, this, C.DestructiveForeground);

        RegisterStyle(NewStyle<SBadge>("SBadge-Outline")
            .Add(SBadge.BackgroundColorProperty, Colors.Transparent)
            .AddResourceAppThemeBinding(SBadge.StrokeProperty, this, C.Border));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Outline>Label")
            .AddResourceAppThemeBinding(Label.TextColorProperty, this, C.Foreground);
    }

    private void RegisterSButtonStyles()
    {
        RegisterStyle(NewStyle<SButton>("SButton")
            .Add(new Trigger(typeof(SButton)) { Property = SButton.IsEnabledProperty, Value = false }
                .Add(SButton.OpacityProperty, 0.5))
            .Add(CursorPointerBehavior.CursorPointerProperty, true)
            .Add(SButton.FontSizeProperty, 14)
            .Add(SButton.FontFamilyProperty, "GeistMedium"));

        RegisterStyle(NewStyle<SButton>("SButton-Primary")
            .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Primary)
            .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.PrimaryForeground)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Primary90)));

        RegisterStyle(NewStyle<SButton>("SButton-Secondary")
            .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Secondary)
            .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.SecondaryForeground)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Secondary80)));

        RegisterStyle(NewStyle<SButton>("SButton-Destructive")
            .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Destructive)
            .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.DestructiveForeground)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Destructive90)));

        RegisterStyle(NewStyle<SButton>("SButton-Outline")
            .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Background)
            .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.Primary)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Accent)
                .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.AccentForeground)));

        RegisterStyle(NewStyle<SButton>("SButton-Ghost")
            .Add(SButton.BackgroundColorProperty, Colors.Transparent)
            .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.Primary)
            .Add(SButton.BorderWidthProperty, 0)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SButton.BackgroundColorProperty, this, C.Accent)
                .AddResourceAppThemeBinding(SButton.TextColorProperty, this, C.AccentForeground)));
    }

    private void RegisterSDatePickerStyles()
    {
        RegisterStyle(NewStyle<SDatePicker>("SDatePicker")
            .AddResourceAppThemeBinding(SDatePicker.BackgroundColorProperty, this, C.Background)
            .Add(
                (CursorPointerBehavior.CursorPointerProperty, true),
                (SDatePicker.FontFamilyProperty, "GeistRegular"))
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SDatePicker.BackgroundColorProperty, this, C.Background90)));
    }

    private void RegisterSCheckboxStyles()
    {
        RegisterStyle(NewStyle<SCheckbox>(nameof(SCheckbox))
            .AddResourceAppThemeBinding(SCheckbox.BackgroundColorProperty, this, C.Background)
            .AddResourceAppThemeBinding(SCheckbox.ColorProperty, this, C.Primary)
            .Add(CursorPointerBehavior.CursorPointerProperty, true));
    }

    private void RegisterSBorderStyles()
    {
        RegisterStyle(NewStyle<SBorder>(nameof(SBorder))
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Card)
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Border)
            .Add(SBorder.CornerRadiusProperty, new CornerRadius(6)));
    }

    private void RegisterSCommandStyles()
    {
        RegisterStyle(NewStyle<SCommandInput>("SCommandInput")
            .Add(SCommandInput.FontFamilyProperty, "GeistRegular")
            .AddResourceAppThemeBinding(SCommandInput.PlaceholderColorProperty, this, C.MutedForeground));

        RegisterStyle(NewStyle<FlexLayout>("SCommandInput-EntryContainer")
            .Add(FlexLayout.MarginProperty, new Thickness(10, 15, 10, 10)));

        RegisterStyle(NewStyle<Entry>("SCommandInput-Entry")
            .Add(EntryExtensionsBehavior.HasUnderLineProperty, false)
            .Add(Entry.BackgroundColorProperty, Colors.Transparent));

        RegisterStyle(NewStyle<SIcon>("SCommandInput-Icon")
            .Add(SIcon.MarginProperty, new Thickness(0, 0, 2, 0))
            .AddResourceAppThemeBinding(SIcon.ColorProperty, this, C.MutedForeground));

        RegisterStyle(NewStyle<SBorder>("SCommandInput-BottomBorder")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Border)
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Border));

        RegisterStyle(NewStyle<SLabel>("SCommandGroup-Heading")
            .Add(SLabel.FontFamilyProperty, "GeistMedium")
            .Add(SLabel.PaddingProperty, new Thickness(14, 10))
            .Add(SLabel.FontSizeProperty, 13)
            .AddResourceAppThemeBinding(SLabel.TextColorProperty, this, C.MutedForeground));

        RegisterStyle(NewStyle<SCommandItem>("SCommandItem")
            .Add(SCommandItem.PaddingProperty, new Thickness(14, 10)));

        RegisterStyle(NewStyle<SBorder>("SCommandItem-Border")
           .BasedOn((Style)this["Shadcn-SBorder"])
           .Add(SBorder.StrokeProperty, Colors.Transparent)
           .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Accent)));

        RegisterSelectorStyle<SLabel>("SCommandItem SLabel")
            .Add(SLabel.FontFamilyProperty, "GeistMedium");

        RegisterStyle(NewStyle<SCommandSeparator>(nameof(SCommandSeparator))
            .Add(SCommandSeparator.HeightRequestProperty, 1)
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Border)
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Border));
    }

    private void RegisterSPopoverStyles()
    {
        RegisterStyle(NewStyle<SBorder>("SPopoverTriggerView")
            .Add(CursorPointerBehavior.CursorPointerProperty, true)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Accent)));
    }

    private void RegisterSSliderStyles()
    {
        RegisterStyle(NewStyle<SBorder>("SSlider-Track")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Secondary)
            .Add(SBorder.CornerRadiusProperty, 9999)
            .Add(SBorder.StrokeThicknessProperty, 0));

        RegisterStyle(NewStyle<SBorder>("SSlider-Range")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Primary)
            .Add(SBorder.StrokeThicknessProperty, 0));

        RegisterStyle(NewStyle<SBorder>("SSlider-Thumb")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Background)
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Primary)
            .Add(SBorder.WidthRequestProperty, 25)
            .Add(SBorder.HeightRequestProperty, 25)
            .Add(SBorder.CornerRadiusProperty, 9999)
            .Add(SBorder.StrokeThicknessProperty, 2));
    }

    private void RegisterSToggleStyles()
    {
        RegisterStyle(NewStyle<SToggle>("SToggle")
            .Add(CursorPointerBehavior.CursorPointerProperty, true)
            .SetPointerOverVisualState((style) => style
                .Add(SToggle.IsPointerOverProperty, true)));

        RegisterStyle(NewStyle<SBorder>("SToggle-Border")
            .Add(SBorder.BackgroundColorProperty, Colors.Transparent)
            .Add(SBorder.CornerRadiusProperty, new CornerRadius(6)));

        RegisterStyle(NewStyle<SBorder>("SToggle-Default")
            .Add(SBorder.StrokeThicknessProperty, 0)
            .Add(SBorder.StrokeProperty, Colors.Transparent)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Muted)));

        RegisterStyle(NewStyle<SBorder>("SToggle-Outline")
            .Add(SBorder.StrokeThicknessProperty, 1)
            .AddResourceAppThemeBinding(SBorder.StrokeProperty, this, C.Input)
            .SetPointerOverVisualState((style) => style
                .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Accent)));

        RegisterSelectorStyle<SBorder>(".Shadcn-SToggle:Value > .Shadcn-SToggle-Border")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Accent);

        RegisterSelectorStyle<SBorder>(".Shadcn-SToggle:IsPointerOver > .Shadcn-SToggle-Border")
            .AddResourceAppThemeBinding(SBorder.BackgroundColorProperty, this, C.Accent);

        RegisterSelectorStyle<SIcon>(".Shadcn-SToggle-Border SIcon")
           .Add(SIcon.TranslationXProperty, -1)
           .AddResourceAppThemeBinding(SIcon.ColorProperty, this, C.Primary);

        RegisterSelectorStyle<SIcon>(".Shadcn-SToggle:Value > .Shadcn-SToggle-Border SIcon", order: 1)
            .AddResourceAppThemeBinding(SIcon.ColorProperty, this, C.AccentForeground);

        RegisterSelectorStyle<SIcon>(".Shadcn-SToggle:IsPointerOver > .Shadcn-SToggle-Border SIcon")
            .AddResourceAppThemeBinding(SIcon.ColorProperty, this, C.MutedForeground);

        RegisterSelectorStyle<SLabel>(".Shadcn-SToggle-Border SLabel")
           .Add(SLabel.FontFamilyProperty, "GeistMedium")
           .AddResourceAppThemeBinding(SLabel.TextColorProperty, this, C.Primary);

        RegisterSelectorStyle<SLabel>(".Shadcn-SToggle:Value > .Shadcn-SToggle-Border SLabel", order: 1)
            .AddResourceAppThemeBinding(SLabel.TextColorProperty, this, C.AccentForeground);

        RegisterSelectorStyle<SLabel>(".Shadcn-SToggle:IsPointerOver > .Shadcn-SToggle-Border SLabel")
            .AddResourceAppThemeBinding(SLabel.TextColorProperty, this, C.MutedForeground);


    }
}
