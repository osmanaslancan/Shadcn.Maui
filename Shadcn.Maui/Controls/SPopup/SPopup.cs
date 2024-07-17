using CommunityToolkit.Maui.Views;

namespace Shadcn.Maui.Controls;

public class SPopup : Popup
{
    public override View? Content
    {
        get => base.Content;
        set => base.Content = value;
    }
}
