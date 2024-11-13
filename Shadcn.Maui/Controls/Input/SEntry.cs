
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
            StyleClass = ["Shadcn-SEntry-Entry"],
        };

        BindWrappedEntry(_entry);
        ControlTemplate = new ControlTemplate(() =>
            new SBorder
            {
                StyleClass = ["Shadcn-SEntry-Ring"],
                Content = new SBorder
                {
                    StyleClass = ["Shadcn-SEntry-Border"],
                    Content = _entry
                }
            });
        
    }
}
