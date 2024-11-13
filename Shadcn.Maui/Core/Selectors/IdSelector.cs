
namespace Shadcn.Maui.Core.Selectors;

internal class IdSelector(string id) : Selector
{
    public override bool Matches(VisualElement styleable)
    {
        return styleable.StyleId == id;
    }

    public override void Bind(VisualElement styleable, Action action)
    {
        BindToProperty(styleable, nameof(VisualElement.StyleId), action);
    }

    public override void UnBind(VisualElement styleable)
    {
        UnBindPropertyListener(styleable);
    }
}
