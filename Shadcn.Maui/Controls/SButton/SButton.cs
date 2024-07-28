using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SButton : Button, IVariantStyleMapper<SButtonVariant>
{
    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant),
        typeof(SButtonVariant),
        typeof(SButton),
        SButtonVariant.Primary,
        propertyChanging: VariantHelpers.VariantPropertyChanging<SButtonVariant>,
        propertyChanged: VariantHelpers.VariantPropertyChanged<SButtonVariant>);

    public SButtonVariant Variant
    {
        get { return (SButtonVariant)GetValue(VariantProperty); }
        set { SetValue(VariantProperty, value); }
    }

    public VisualElement VariantElement => this;

    public string MapVariant(SButtonVariant variant)
    {
        return "Shadcn-" + variant switch
        {
            SButtonVariant.Primary => "SButton-Primary",
            SButtonVariant.Secondary => "SButton-Secondary",
            SButtonVariant.Destructive => "SButton-Destructive",
            SButtonVariant.Outline => "SButton-Outline",
            SButtonVariant.Ghost => "SButton-Ghost",
            _ => "SButton-Primary",
        };
    }
    
    public SButton()
    {
        StyleClass = ["Shadcn-SButton"];
        this.AddVariantStyleClass(SButtonVariant.Primary);
    }
}
