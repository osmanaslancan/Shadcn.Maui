using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Markup;

namespace Shadcn.Maui.Controls;

public static class ShadcnMauiAppBuilderExtensions
{
    public static MauiAppBuilder UseShadcnMauiControls(this MauiAppBuilder builder)
    {
        builder.UseMauiCommunityToolkitMarkup();
        builder.UseMauiCommunityToolkit();

        return builder;
    }
}
