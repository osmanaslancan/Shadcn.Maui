using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Common;
using Shadcn.Maui.Resources;

namespace Shadcn.Maui.Controls;

[WrapsControl(typeof(Entry))]
public partial class SCommandInput : TemplatedView
{
    public SCommandInput()
    {
        StyleClass = ["Shadcn-SCommandInput"];
        ControlTemplate = new ControlTemplate(() =>
        {
            return new VerticalStackLayout()
            {
                Children =
                {
                    new FlexLayout
                    {
                        StyleClass = ["Shadcn-SCommandInput-EntryContainer"],
                        AlignItems = Microsoft.Maui.Layouts.FlexAlignItems.Center,
                        Children =
                        {
                            new SIcon()
                            {
                                StyleClass = ["Shadcn-SCommandInput-Icon"],
                                Icon = Icons.Search
                            },
                            new Entry
                            {
                                StyleClass = ["Shadcn-SCommandInput-Entry"],
                            }.Invoke(x => BindWrappedEntry(x)).Grow(1)
                        }
                    },
                    new SBorder()
                    {
                        HeightRequest = 1,
                        StyleClass = ["Shadcn-SCommandInput-BottomBorder"],
                        Content = new Grid()
                    }
                }
            };
        });
    }
}
