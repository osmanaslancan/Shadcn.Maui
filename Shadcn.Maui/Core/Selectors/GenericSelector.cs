namespace Shadcn.Maui.Core.Selectors;

internal class GenericSelector(
    Func<VisualElement, bool> matchesAction,
    Action<VisualElement, Action> bindAction,
    Action<VisualElement> unbindAction) : Selector
{
    public GenericSelector(Func<VisualElement, bool> matchesAction)
        : this(matchesAction, (styleable, action) => { }, styleable => { })
    {
    }

    public override bool Matches(VisualElement styleable)
    {
        return matchesAction(styleable);
    }

    public override void Bind(VisualElement styleable, Action action)
    {
        bindAction(styleable, action);
    }

    public override void UnBind(VisualElement styleable)
    {
        unbindAction(styleable);
    }
}
