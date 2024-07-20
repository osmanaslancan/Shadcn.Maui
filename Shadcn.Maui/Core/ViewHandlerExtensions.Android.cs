using Microsoft.Maui.Handlers;

namespace Shadcn.Maui.Core;

public static partial class ViewHandlerExtensions
{
    public static void ProgrammaticClick(this ViewHandler viewHandler)
    {
        viewHandler.PlatformView!.PerformClick();
    }
}
