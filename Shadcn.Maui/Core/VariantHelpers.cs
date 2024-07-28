namespace Shadcn.Maui.Core;

internal static class VariantHelpers
{
    public static T AddStyleClass<T>(this T visualElement, string styleClass)
        where T : VisualElement
    {
        visualElement.StyleClass ??= [];

        visualElement.StyleClass = [.. visualElement.StyleClass, styleClass];

        return visualElement;
    }

    public static T RemoveStyleClass<T>(this T visualElement, string styleClass)
        where T : VisualElement
    {
        visualElement.StyleClass.Remove(styleClass);
        visualElement.StyleClass = [..visualElement.StyleClass];

        return visualElement;
    }

    public static void AddVariantStyleClass<T, TVariant>(this T visualElement, TVariant variant)
        where T : VisualElement, IVariantStyleMapper<TVariant>
    {
        visualElement.VariantElement.StyleClass ??= [];

        visualElement.VariantElement.StyleClass = [..visualElement.VariantElement.StyleClass, visualElement.MapVariant(variant)];
    }

    public static void RemoveVariantStyleClass<T, TVariant>(this T visualElement, TVariant variant)
        where T : VisualElement, IVariantStyleMapper<TVariant>
    {
        visualElement.VariantElement.StyleClass.Remove(visualElement.MapVariant(variant));
        visualElement.VariantElement.StyleClass = [..visualElement.VariantElement.StyleClass];
    }

    public static void VariantPropertyChanging<T>(BindableObject bindable, object oldValue, object newValue)
    {
        var mapper = (IVariantStyleMapper<T>)bindable;

        if (oldValue is T oldVariant)
            mapper.VariantElement.RemoveStyleClass(mapper.MapVariant(oldVariant));
    }

    public static void VariantPropertyChanged<T>(BindableObject bindable, object oldValue, object newValue)
    {
        var mapper = (IVariantStyleMapper<T>)bindable;

        if (newValue is T newVariant)
            mapper.VariantElement.AddStyleClass(mapper.MapVariant(newVariant));
    }
}
