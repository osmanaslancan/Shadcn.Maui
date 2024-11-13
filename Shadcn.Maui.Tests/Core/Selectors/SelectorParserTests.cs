namespace Shadcn.Maui.Core.Selectors;

public class SelectorParserTests
{
    [Fact]
    public void Parse_Should_Throw_Exception_When_Selector_Empty()
    {
        Assert.Throws<ArgumentException>(() => SelectorParser.Parse(""));
    }

    [Fact]
    public void Parse_Should_Return_All_Selector_When_Selector_Is_Asterisk()
    {
        var selector = SelectorParser.Parse("*");
        Assert.True(selector.Matches(new Border()));
    }

    [Fact]
    public void Parse_Should_Return_Selector_When_Selector_Is_Type()
    {
        var selector = SelectorParser.Parse("Border");
        Assert.True(selector.Matches(new Border()));
        Assert.False(selector.Matches(new Label()));
    }

    [Fact]
    public void Parse_Should_Return_Selector_When_Selector_Is_Type_And_Class()
    {
        var selector = SelectorParser.Parse("Border.Class");
        Assert.True(selector.Matches(new Border { StyleClass = ["Class"] }));
        Assert.False(selector.Matches(new Border()));
    }

    [Theory]
    [InlineData("Border#Id")]
    [InlineData("#Id")]
    public void Parse_Should_Return_Selector_When_Selector_Is_Type_And_Id(string data)
    {
        var selector = SelectorParser.Parse(data);
        Assert.True(selector.Matches(new Border { StyleId = "Id" }));
        Assert.False(selector.Matches(new Border()));
    }
}
