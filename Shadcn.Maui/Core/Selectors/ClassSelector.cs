
namespace Shadcn.Maui.Core.Selectors;

internal class ClassSelector(string className) : Selector
{
    public override bool Matches(VisualElement styleable)
    {
        return styleable.StyleClass != null && styleable.StyleClass.Contains(className);
    }

    public override void Bind(VisualElement styleable, Action action)
    {
        BindToProperty(styleable, nameof(VisualElement.StyleClass), action);
    }

    public override void UnBind(VisualElement styleable)
    {
        UnBindPropertyListener(styleable);
    }
}
