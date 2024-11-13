namespace Shadcn.Maui.Core.Selectors;

internal class OrOperator : Operator
{
    public override bool Matches(VisualElement styleable)
    {
        return Right.Matches(styleable) || Left.Matches(styleable);
    }
}
