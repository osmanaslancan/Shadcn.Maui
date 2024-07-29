using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SBadge : SBorder, IVariantStyleMapper<SBadgeVariant>
{
    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(SBadgeVariant),
        typeof(SBadge),
        SBadgeVariant.Primary,
        propertyChanging: VariantHelpers.VariantPropertyChanging<SBadgeVariant>,
        propertyChanged: VariantHelpers.VariantPropertyChanged<SBadgeVariant>);

    public SBadgeVariant Variant
    {
        get => (SBadgeVariant)GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public VisualElement VariantElement => this;

    public string MapVariant(SBadgeVariant variant)
    {
        return "Shadcn-" + variant switch
        {
            SBadgeVariant.Primary => "SBadge-Primary",
            SBadgeVariant.Secondary => "SBadge-Secondary",
            SBadgeVariant.Destructive => "SBadge-Destructive",
            SBadgeVariant.Outline => "SBadge-Outline",
            _ => "SBadge-Primary",
        };
    }

    public SBadge()
    {
        StyleClass = ["Shadcn-SBadge"];
        this.AddVariantStyleClass(SBadgeVariant.Primary);
    }
}
