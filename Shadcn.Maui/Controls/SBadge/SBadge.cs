namespace Shadcn.Maui.Controls;

public class SBadge : SBorder
{
    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(SBadgeVariant),
        typeof(SBadge),
        SBadgeVariant.Primary,
        propertyChanging: VariantChanging,
        propertyChanged: VariantChanged);

    public SBadgeVariant Variant
    {
        get => (SBadgeVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    private string GetVariantStyleClass(SBadgeVariant variant)
    {
        return variant switch
        {
            SBadgeVariant.Primary => "SBadge-Primary",
            SBadgeVariant.Secondary => "SBadge-Secondary",
            SBadgeVariant.Destructive => "SBadge-Destructive",
            SBadgeVariant.Outline => "SBadge-Outline",
            _ => "SBadge-Primary",
        };
    }

    private void AddVariantStyleClass(SBadgeVariant variant)
    {
        var styleClass = "Shadcn-" + GetVariantStyleClass(variant);

        StyleClass ??= [];

        StyleClass = [..StyleClass, styleClass];
    }

    private void RemoveVariantStyleClass(SBadgeVariant variant)
    {
        if (StyleClass is null)
            return;

        StyleClass.Remove("Shadcn-" + GetVariantStyleClass(variant));
    }

    private static void VariantChanging(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SBadge badge)
            return;

        if (oldValue is SBadgeVariant oldVariant)
            badge.RemoveVariantStyleClass(oldVariant);
    }

    private static void VariantChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SBadge badge)
            return;

        if (newValue is SBadgeVariant newVariant)
            badge.AddVariantStyleClass(newVariant);
    }

    public SBadge()
    {
        StyleClass = ["Shadcn-SBadge"];
        AddVariantStyleClass(SBadgeVariant.Primary);
    }
}
