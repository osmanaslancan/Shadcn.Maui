namespace Shadcn.Maui.Core;

public interface IVariantStyleMapper<T>
{
    VisualElement VariantElement { get; }
    string MapVariant(T variant);
}
