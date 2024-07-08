namespace Shadcn.Maui.Controls;

public class SButton : Button
{
    public ButtonVariant Variant
    {
        get { return (ButtonVariant)GetValue(VariantProperty); }
        set { SetValue(VariantProperty, value); }
    }

    public bool IsHovering
    {
        get { return (bool)GetValue(IsHoveringProperty); }
        set { SetValue(IsHoveringProperty, value); }
    }


    public static readonly BindableProperty VariantProperty = BindableProperty.Create(nameof(Variant), typeof(ButtonVariant), typeof(SButton), default(string));
    public static readonly BindableProperty IsHoveringProperty = BindableProperty.Create(nameof(IsHovering), typeof(bool), typeof(SButton), default(bool));

    public SButton()
    {

    }
}
