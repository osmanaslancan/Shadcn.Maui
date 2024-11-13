using System.Text.RegularExpressions;

namespace Shadcn.Maui.Core.Selectors;

public partial class SelectorParser
{
    internal static Selector All = new GenericSelector(_ => true);
    internal static Selector None = new GenericSelector(_ => false);

    public static Selector Parse(ReadOnlySpan<char> selector)
    {
        return new SelectorParser().InternalParse(selector);
    }

    private SelectorParser()
    {
    }

    private int index;
    private Selector head = All;

    private Selector AddStep<T>(Selector selector)
        where T : Operator, new()
    {
        var @operator = new T();
        @operator.Left = head;
        @operator.Right = selector;
        head = @operator.Right;
        return head;
    }

    private Selector InternalParse(ReadOnlySpan<char> selector)
    {
        if (selector.IsEmpty)
        {
            throw new ArgumentException("Selector cannot be empty");
        }

        index = 0;

        while (index < selector.Length)
        {
            var c = selector[index];

            if (c == '.')
            {
                index++;
                var className = FindNextName(selector);

                AddStep<AndOperator>(new ClassSelector(className.ToString()));

                index += className.Length;
            }
            else if (c == '*')
            {
                AddStep<AndOperator>(All);
                index++;
            }
            else if (c == '#')
            {
                index++;
                var idName = FindNextName(selector);

                AddStep<AndOperator>(new IdSelector(idName.ToString()));

                index += idName.Length;
            }
            else if (char.IsWhiteSpace(c))
            {
                index++;
                //TODO(osman): AddDescendantSelector
            }
            else if (c == ',')
            {
                AddStep<OrOperator>(All);
                index++;
            }
            else if (c == '>')
            {
                AddStep<ChildOperator>(All);
                index++;
            }
            else
            {
                var name = FindNextName(selector);
                AddStep<AndOperator>(new ElementSelector(name.ToString()));
                index += name.Length;
            }
        }

        return head;
    }
   
    private ReadOnlySpan<char> FindNextName(ReadOnlySpan<char> selector)
    {
        var name = FindNextNameOrDefault(selector);

        if (name.IsEmpty)
        {
            throw new ParserException(index, "Invalid Name");
        }

        return name;
    }

    private ReadOnlySpan<char> FindNextNameOrDefault(ReadOnlySpan<char> selector)
    {
        ReadOnlySpan<char> className = default;
        foreach (var c in NameRegex().EnumerateMatches(selector[index..]))
        {
            className = selector.Slice(c.Index, c.Length);
            break;
        }

        return className;
    }

    [GeneratedRegex("-?[_a-zA-Z]+[_a-zA-Z0-9-]*")]
    private static partial Regex NameRegex();
}
