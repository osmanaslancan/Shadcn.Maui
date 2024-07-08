using Shadcn.Maui.Common;

namespace Shadcn.Maui.Resources;

[ColorDictionary]
public partial class Colors : ResourceDictionary
{
	public Colors()
	{
		InitializeComponent();
        AddOpacities();
    }
}