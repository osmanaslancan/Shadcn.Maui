
using Shadcn.Maui.Common;

namespace Shadcn.Maui.Controls;

[WrapsControl(typeof(Entry))]
public partial class SEntry : ContentView
{
    private Entry _entry;

    public SEntry()
    {
        _entry = new Entry();
        BindWrappedEntry(_entry);
        Content = _entry;
    }
}
