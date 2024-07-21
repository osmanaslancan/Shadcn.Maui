
using Shadcn.Maui.Common;

namespace Shadcn.Maui.Controls;

[WrapsControl(typeof(Entry))]
public partial class SEntry : TemplatedView
{
    private readonly Entry _entry;

    public Entry WrappedEntry => _entry;

    public SEntry()
    {
        _entry = new Entry()
        {
            StyleClass = ["SEntry-Entry"],
        };

        BindWrappedEntry(_entry);
        ControlTemplate = new ControlTemplate(() =>
            new Border
            {
                StyleClass = ["SEntry-Ring"],
                Content = new Border
                {
                    StyleClass = ["SEntry-Border"],
                    Content = _entry
                }
            });
    }
}
