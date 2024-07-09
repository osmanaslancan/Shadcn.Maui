
using Shadcn.Maui.Common;

namespace Shadcn.Maui.Controls;

[WrapsControl(typeof(Entry))]
public partial class SEntry : ContentView
{
    private readonly Entry _entry;

    public Entry WrappedEntry => _entry;

    public SEntry()
    {
        _entry = new Entry()
        {
            StyleClass = ["SEntry-Entry"]
        };
        
        BindWrappedEntry(_entry);
        Content = new Border
        {
            StyleClass = ["SEntry-Border"],
            Content = _entry
        };
    }
}
