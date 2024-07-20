using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Handlers;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Shadcn.Maui.Core;

public static partial class ViewHandlerExtensions
{
    public static void ProgrammaticClick(this ViewHandler viewHandler)
    {
        if (viewHandler.PlatformView is ButtonBase button)
        {
            var peer = ButtonBaseAutomationPeer.CreatePeerForElement(button);
            if (peer is ButtonAutomationPeer buttonPeer)
            {
                buttonPeer.Invoke();
            }
            else if (peer is ToggleButtonAutomationPeer toggleButtonPeer)
            {
                toggleButtonPeer.Toggle();
            }
        }
    }
}
