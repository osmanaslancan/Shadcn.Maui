
using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Layouts;
using Shadcn.Maui.Behaviors;
using Shadcn.Maui.Controls;
using Shadcn.Maui.Core;
using System.Diagnostics;

namespace Shadcn.Maui.Resources;

public partial class StyleResource : ResourceDictionary
{

    public StyleResource()
    {
        InitializeComponent();
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
                return new SBorder()
                {
                    CornerRadius = new CornerRadius(6),
                    Content = new StackLayout
                    {
                        StyleClass = ["Shadcn-SCard-StackLayout"],
                        VerticalOptions = LayoutOptions.Start,
                    }.Bind(BindableLayout.ItemsSourceProperty, nameof(SCard.Children), source: RelativeBindingSource.TemplatedParent)
                    .SetBindableValue(BindableLayout.ItemTemplateProperty, new DataTemplate(() => new ContentPresenter().Bind(ContentPresenter.ContentProperty)))
                }.AppThemeBinding(SBorder.BackgroundColorProperty, GetColor("Card"), GetColor("DarkCard"))
                .AppThemeBinding(SBorder.StrokeProperty, GetColor("Border"), GetColor("DarkBorder"))
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
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("Primary"), GetColor("DarkPrimary"));

        RegisterSelectorStyle<Label>("SCardDescription Label")
            .Add(
                (Label.FontSizeProperty, 14))
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("MutedForeground"), GetColor("DarkMutedForeground"));
    }

    private void RegisterSAlertDialogStyles()
    {
    }

    private void RegisterSAvatarStyles()
    {
        RegisterStyle(NewStyle<SAvatar>("SAvatar")
            .Add(SAvatar.BorderWidthProperty, 0)
            .Add(SAvatar.FontFamilyProperty, "Geist")
            .AddAppThemeBinding(SAvatar.BackgroundColorProperty, GetColor("Muted"), GetColor("DarkMuted")));
    }

    private void RegisterSBadgeStyles()
    {
        RegisterStyle(NewStyle<SBadge>("SBadge")
            .Add(SBadge.CornerRadiusProperty, new CornerRadius(99999))
            .Add(SBadge.PaddingProperty, new Thickness(10,2)));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge>Label")
            .Add(Label.FontSizeProperty, 14)
            .Add(Label.FontFamilyProperty, "GeistSemiBold");

        RegisterStyle(NewStyle<SBadge>("SBadge-Primary")
            .AddAppThemeBinding(SBadge.BackgroundColorProperty, GetColor("Primary"), GetColor("DarkPrimary"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SBadge.BackgroundColorProperty, GetColor("Primary80"), GetColor("DarkPrimary80"))));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Primary>Label")
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("PrimaryForeground"), GetColor("DarkPrimaryForeground"));

        RegisterStyle(NewStyle<SBadge>("SBadge-Secondary")
            .AddAppThemeBinding(SBadge.BackgroundColorProperty, GetColor("Secondary"), GetColor("DarkSecondary"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SBadge.BackgroundColorProperty, GetColor("Secondary80"), GetColor("DarkSecondary80"))));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Secondary>Label")
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("SecondaryForeground"), GetColor("DarkSecondaryForeground"));

        RegisterStyle(NewStyle<SBadge>("SBadge-Destructive")
            .AddAppThemeBinding(SBadge.BackgroundColorProperty, GetColor("Destructive"), GetColor("DarkDestructive"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SBadge.BackgroundColorProperty, GetColor("Destructive80"), GetColor("DarkDestructive80"))));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Destructive>Label")
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("DestructiveForeground"), GetColor("DarkDestructiveForeground"));

        RegisterStyle(NewStyle<SBadge>("SBadge-Outline")
            .Add(SBadge.BackgroundColorProperty, Colors.Transparent)
            .AddAppThemeBinding(SBadge.StrokeProperty, GetColor("Border"), GetColor("DarkBorder")));

        RegisterSelectorStyle<Label>(".Shadcn-SBadge-Outline>Label")
            .AddAppThemeBinding(Label.TextColorProperty, GetColor("Foreground"), GetColor("DarkForeground"));
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
            .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Primary"), GetColor("DarkPrimary"))
            .AddAppThemeBinding(SButton.TextColorProperty, GetColor("PrimaryForeground"), GetColor("DarkPrimaryForeground"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Primary90"), GetColor("DarkPrimary90"))));

        RegisterStyle(NewStyle<SButton>("SButton-Secondary")
            .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Secondary"), GetColor("DarkSecondary"))
            .AddAppThemeBinding(SButton.TextColorProperty, GetColor("SecondaryForeground"), GetColor("DarkSecondaryForeground"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Secondary80"), GetColor("DarkSecondary80"))));

        RegisterStyle(NewStyle<SButton>("SButton-Destructive")
            .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Destructive"), GetColor("DarkDestructive"))
            .AddAppThemeBinding(SButton.TextColorProperty, GetColor("DestructiveForeground"), GetColor("DarkDestructiveForeground"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Destructive90"), GetColor("DarkDestructive90"))));

        RegisterStyle(NewStyle<SButton>("SButton-Outline")
            .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Background"), GetColor("DarkBackground"))
            .AddAppThemeBinding(SButton.TextColorProperty, GetColor("Primary"), GetColor("DarkPrimary"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Accent"), GetColor("DarkAccent"))
                .AddAppThemeBinding(SButton.TextColorProperty, GetColor("AccentForeground"), GetColor("DarkAccentForeground"))));

        RegisterStyle(NewStyle<SButton>("SButton-Ghost")
            .AddAppThemeBinding(SButton.BackgroundColorProperty, Colors.Transparent, Colors.Transparent)
            .AddAppThemeBinding(SButton.TextColorProperty, GetColor("Primary"), GetColor("DarkPrimary"))
            .Add(SButton.BorderWidthProperty, 0)
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SButton.BackgroundColorProperty, GetColor("Accent"), GetColor("DarkAccent"))
                .AddAppThemeBinding(SButton.TextColorProperty, GetColor("AccentForeground"), GetColor("DarkAccentForeground"))));
    }

    private void RegisterSDatePickerStyles()
    {
        RegisterStyle(NewStyle<SDatePicker>("SDatePicker")
            .AddAppThemeBinding(SDatePicker.BackgroundColorProperty, GetColor("Background"), GetColor("DarkBackground"))
            .Add(
                (CursorPointerBehavior.CursorPointerProperty, true),
                (SDatePicker.FontFamilyProperty, "GeistRegular"))
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SDatePicker.BackgroundColorProperty, GetColor("Background90"), GetColor("DarkBackground90"))));
    }

    private void RegisterSCheckboxStyles()
    {
        RegisterStyle(NewStyle<SCheckbox>(nameof(SCheckbox))
            .AddAppThemeBinding(SCheckbox.BackgroundColorProperty, GetColor("Background"), GetColor("DarkBackground"))
            .AddAppThemeBinding(SCheckbox.ColorProperty, GetColor("Primary"), GetColor("DarkPrimary"))
            .Add(CursorPointerBehavior.CursorPointerProperty, true));
    }

    private void RegisterSBorderStyles()
    {
        RegisterStyle(NewStyle<SBorder>(nameof(SBorder))
            .AddAppThemeBinding(SBorder.BackgroundColorProperty, GetColor("Card"), GetColor("DarkCard"))
            .AddAppThemeBinding(SBorder.StrokeProperty, GetColor("Border"), GetColor("DarkBorder"))
            .Add(SBorder.CornerRadiusProperty, new CornerRadius(6)));
    }

    private void RegisterSCommandStyles()
    {
        RegisterStyle(NewStyle<SCommandInput>("SCommandInput")
            .Add(SCommandInput.FontFamilyProperty, "GeistRegular")
            .AddAppThemeBinding(SCommandInput.PlaceholderColorProperty, GetColor("MutedForeground"), GetColor("DarkMutedForeground")));

        RegisterStyle(NewStyle<FlexLayout>("SCommandInput-EntryContainer")
            .Add(FlexLayout.MarginProperty, new Thickness(10, 15, 10, 10)));

        RegisterStyle(NewStyle<Entry>("SCommandInput-Entry")
            .Add(EntryExtensionsBehavior.HasUnderLineProperty, false)
            .Add(Entry.BackgroundColorProperty, Colors.Transparent));

        RegisterStyle(NewStyle<SIcon>("SCommandInput-Icon")
            .Add(SIcon.MarginProperty, new Thickness(0,0,2,0))
            .AddAppThemeBinding(SIcon.ColorProperty, GetColor("MutedForeground"), GetColor("DarkMutedForeground")));

        RegisterStyle(NewStyle<SBorder>("SCommandInput-BottomBorder")
            .AddAppThemeBinding(SBorder.BackgroundColorProperty, GetColor("Border"), GetColor("DarkBorder"))
            .AddAppThemeBinding(SBorder.StrokeProperty, GetColor("Border"), GetColor("DarkBorder")));

        RegisterStyle(NewStyle<SLabel>("SCommandGroup-Heading")
            .Add(SLabel.FontFamilyProperty, "GeistMedium")
            .Add(SLabel.PaddingProperty, new Thickness(14, 10))
            .Add(SLabel.FontSizeProperty, 13)
            .AddAppThemeBinding(SLabel.TextColorProperty, GetColor("MutedForeground"), GetColor("DarkMutedForeground")));

        RegisterStyle(NewStyle<SCommandItem>("SCommandItem")
            .Add(SCommandItem.PaddingProperty, new Thickness(14, 10)));

        RegisterStyle(NewStyle<SBorder>("SCommandItem-Border")
           .BasedOn((Style)this["Shadcn-SBorder"])
           .Add(SBorder.StrokeProperty, Colors.Transparent)
           .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SBorder.BackgroundColorProperty, GetColor("Accent"), GetColor("DarkAccent"))));

        RegisterSelectorStyle<SLabel>(".Shadcn-SCommandItem>SLabel")
            .Add(SLabel.FontFamilyProperty, "GeistMedium");

        RegisterStyle(NewStyle<SCommandSeparator>(nameof(SCommandSeparator))
            .Add(SCommandSeparator.HeightRequestProperty, 1)
            .AddAppThemeBinding(SBorder.BackgroundColorProperty, GetColor("Border"), GetColor("DarkBorder"))
            .AddAppThemeBinding(SBorder.StrokeProperty, GetColor("Border"), GetColor("DarkBorder")));
    }

    private void RegisterSPopoverStyles()
    {
        RegisterStyle(NewStyle<SBorder>("SPopoverTriggerView")
            .Add(CursorPointerBehavior.CursorPointerProperty, true)
            .SetPointerOverVisualState((style) => style
                .AddAppThemeBinding(SBorder.BackgroundColorProperty, GetColor("Accent"), GetColor("DarkAccent"))));
    }
}