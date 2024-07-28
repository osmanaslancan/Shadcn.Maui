    using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SToggle : ContentView, IVariantStyleMapper<SToggleVariant>
{
    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
      nameof(Value),
      typeof(bool),
      typeof(SToggle),
      false);

    public bool Value
    {
        get { return (bool)GetValue(ValueProperty); }
        set { SetValue(ValueProperty, value); }
    }

    public static readonly BindableProperty IsPointerOverProperty = BindableProperty.Create(
      nameof(IsPointerOver),
      typeof(bool),
      typeof(SToggle),
      false);

    public bool IsPointerOver
    {
        get { return (bool)GetValue(IsPointerOverProperty); }
        set { SetValue(IsPointerOverProperty, value); }
    }

    public static readonly BindableProperty VariantProperty = BindableProperty.Create(
      nameof(Variant),
      typeof(SToggleVariant),
      typeof(SButton),
      SToggleVariant.Default,
      propertyChanging: VariantHelpers.VariantPropertyChanging<SToggleVariant>,
      propertyChanged: VariantHelpers.VariantPropertyChanged<SToggleVariant>);

    public SToggleVariant Variant
    {
        get { return (SToggleVariant)GetValue(VariantProperty); }
        set { SetValue(VariantProperty, value); }
    }

    public VisualElement VariantElement => _border;

    public string MapVariant(SToggleVariant variant)
    {
        return "Shadcn-SToggle-" + variant switch
        {
            SToggleVariant.Default => "Default",
            SToggleVariant.Outline => "Outline",
            _ => "Default",
        };
    }

    private SBorder _border = default!;

    public SToggle()
    {
        StyleClass = ["Shadcn-SToggle"];
        ControlTemplate = new ControlTemplate(() =>
        {
            return new SBorder()
            {
                StyleClass = ["Shadcn-SToggle-Border"],
            }
            .Bind(SBorder.ContentProperty, nameof(Content), source: this)
            .TapGesture(() => Value = !Value)
            .Assign(out _border);
        });

        this.AddVariantStyleClass(SToggleVariant.Default);
    }
}
