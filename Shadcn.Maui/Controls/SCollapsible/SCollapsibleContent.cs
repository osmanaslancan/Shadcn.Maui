using CommunityToolkit.Maui.Markup;
using Shadcn.Maui.Core;

namespace Shadcn.Maui.Controls;

public class SCollapsibleContent : ContentView
{
    public SCollapsibleContent()
    {
        this.Loaded += (s, e) =>
        {
            var parentCollapsible = this.FindParentOfType<SCollapsible>();

            if (parentCollapsible != null)
            {
                this.Bind(SCollapsibleContent.IsVisibleProperty, nameof(SCollapsible.IsCollapsed), source: parentCollapsible, convert: static (bool isCollapsed) => !isCollapsed);
            }
        };
    }
}
