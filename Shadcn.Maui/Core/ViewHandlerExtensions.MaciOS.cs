using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;
using UIKit;

namespace Shadcn.Maui.Core;

public static partial class ViewHandlerExtensions
{
    public static void ProgrammaticClick(this ViewHandler viewHandler)
    {
        if (viewHandler.PlatformView is MauiCheckBox button)
        {
            button.IsChecked = !button.IsChecked;
        }
    }
}
