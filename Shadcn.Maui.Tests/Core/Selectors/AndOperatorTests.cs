namespace Shadcn.Maui.Core.Selectors;

public class AndOperatorTests
{
    [Fact]
    public void Matches_ReturnsTrue_WhenBothSelectorsMatch()
    {
        var leftSelector = new GenericSelector(s => true);
        var rightSelector = new GenericSelector(s => true);
        var andOperator = new AndOperator
        {
            Left = leftSelector,
            Right = rightSelector
        };
        var visualElement = new Border();

        var result = andOperator.Matches(visualElement);

        Assert.True(result);
    }

    [Fact]
    public void Matches_ReturnsFalse_WhenLeftSelectorDoesNotMatch()
    {
        var leftSelector = new GenericSelector(s => false);
        var rightSelector = new GenericSelector(s => true);
        var andOperator = new AndOperator
        {
            Left = leftSelector,
            Right = rightSelector
        };
        var visualElement = new Border();

        var result = andOperator.Matches(visualElement);

        Assert.False(result);
    }

    [Fact]
    public void Matches_ReturnsFalse_WhenRightSelectorDoesNotMatch()
    {
        var leftSelector = new GenericSelector(s => true);
        var rightSelector = new GenericSelector(s => false);
        var andOperator = new AndOperator
        {
            Left = leftSelector,
            Right = rightSelector
        };
        var visualElement = new Border();

        var result = andOperator.Matches(visualElement);

        Assert.False(result);
    }

    [Fact]
    public void Matches_ReturnsFalse_WhenBothSelectorsDoNotMatch()
    {
        var leftSelector = new GenericSelector(s => false);
        var rightSelector = new GenericSelector(s => false);
        var andOperator = new AndOperator
        {
            Left = leftSelector,
            Right = rightSelector
        };
        var visualElement = new Border();

        var result = andOperator.Matches(visualElement);

        Assert.False(result);
    }

    [Fact]
    public void Binds_Both_Selectors()
    {
        bool leftBind = false, leftUnbind = false, rightBind = false, rightUnbind = false;
        var leftSelector = new GenericSelector(s => true, (_, _) => { leftBind = true; }, (_) => { leftUnbind = false; });
        var rightSelector = new GenericSelector(s => true, (_, _) => { rightBind = true; }, (_) => { rightUnbind = false; });
        var andOperator = new AndOperator
        {
            Left = leftSelector,
            Right = rightSelector
        };
        var visualElement = new Border();
        andOperator.Bind(visualElement, () => { });
        Assert.True(leftBind);
        Assert.True(rightBind);
        Assert.False(leftUnbind);
        Assert.False(rightUnbind);
    }
}
