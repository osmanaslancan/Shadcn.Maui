namespace Shadcn.Maui.Core.Selectors;

internal class ChildOperator : Operator
{
    public override bool Matches(VisualElement styleable)
    {
        return Right.Matches(styleable) && Left.Matches((VisualElement)styleable.Parent);
    }

    public override void Bind(VisualElement styleable, Action action)
    {
        Right.Bind(styleable, action);
        BindToProperty(styleable, nameof(VisualElement.Parent), action);

        if (styleable.Parent is VisualElement parent)
            Left.Bind(parent, action);
    }

    public override void UnBind(VisualElement styleable)
    {
        Right.UnBind(styleable);
        UnBindPropertyListener(styleable);

        if (styleable.Parent is VisualElement parent)
            Left.UnBind(parent);
    }
}
