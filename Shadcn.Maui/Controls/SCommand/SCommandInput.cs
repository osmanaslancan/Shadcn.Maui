using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Common;
using Shadcn.Maui.Core;
using Shadcn.Maui.Resources;

namespace Shadcn.Maui.Controls;

[WrapsControl(typeof(Entry))]
public partial class SCommandInput : TemplatedView
{
    private Entry? _entry;

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
                            }.Invoke(x => BindWrappedEntry(x)).Grow(1).Assign(out _entry)
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

        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        var parentCommand = this.FindParentOfType<SCommand>();

        _entry!.Bind(Entry.TextProperty, nameof(SCommand.SearchText), source: parentCommand, mode: BindingMode.TwoWay);
    }
}
