namespace Shadcn.Maui.Core.Selectors;

internal abstract class Operator : Selector
{
    public Selector Left { get; set; } = SelectorParser.None;
    public Selector Right { get; set; } = SelectorParser.None;

    public override void Bind(VisualElement styleable, Action action)
    {
        Right.Bind(styleable, action);
        Left.Bind(styleable, action);
    }

    public override void UnBind(VisualElement styleable)
    {
        Right.UnBind(styleable);
        Left.UnBind(styleable);
    }
}
