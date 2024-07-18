namespace Shadcn.Maui.Controls;

public class SButton : Button
{
    public SButtonVariant Variant
    {
        get { return (SButtonVariant)GetValue(VariantProperty); }
        set { SetValue(VariantProperty, value); }
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
        nameof(Variant), 
        typeof(SButtonVariant), 
        typeof(SButton), 
        SButtonVariant.Primary,
        propertyChanging: VariantChanging,
        propertyChanged: VariantChanged);

    private string GetVariantStyleClass(SButtonVariant variant)
    {
        return variant switch
        {
            SButtonVariant.Primary => "SButton-Primary",
            SButtonVariant.Secondary => "SButton-Secondary",
            SButtonVariant.Destructive => "SButton-Destructive",
            SButtonVariant.Outline => "SButton-Outline",
            SButtonVariant.Ghost => "SButton-Ghost",
            _ => "SButton-Primary",
        };
    }

    private void AddVariantStyleClass(SButtonVariant variant)
    {
        var styleClass = "Shadcn-" + GetVariantStyleClass(variant);

        StyleClass ??= [];

        StyleClass = [.. StyleClass, styleClass];
    }

    private void RemoveVariantStyleClass(SButtonVariant variant)
    {
        if (StyleClass is null)
            return;

        StyleClass.Remove("Shadcn-" + GetVariantStyleClass(variant));
    }

    private static void VariantChanging(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SButton badge)
            return;

        if (oldValue is SButtonVariant oldVariant)
            badge.RemoveVariantStyleClass(oldVariant);
    }

    private static void VariantChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not SButton badge)
            return;

        if (newValue is SButtonVariant newVariant)
            badge.AddVariantStyleClass(newVariant);
    }

    public SButton()
    {
        StyleClass = ["Shadcn-SButton"];
        AddVariantStyleClass(SButtonVariant.Primary);
    }
}
