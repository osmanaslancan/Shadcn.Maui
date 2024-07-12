using Shadcn.Maui.Core;
using System.ComponentModel;

namespace Shadcn.Maui.Controls;

[ContentProperty(nameof(Content))]
public class SCardTitle : ContentView
{
    [TypeConverter(typeof(StringToLabelTypeConverter))]
    public new View Content
    {
        get { return (View)GetValue(ContentProperty); }
        set { SetValue(ContentProperty, value); }
    }

    public SCardTitle()
	{
	}
}