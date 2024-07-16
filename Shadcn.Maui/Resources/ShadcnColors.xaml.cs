using Shadcn.Maui.Common;

namespace Shadcn.Maui.Resources;

[ColorDictionary]
public partial class ShadcnColors : ResourceDictionary
{
	public ShadcnColors()
	{
		InitializeComponent();
        AddOpacities();
    }
}