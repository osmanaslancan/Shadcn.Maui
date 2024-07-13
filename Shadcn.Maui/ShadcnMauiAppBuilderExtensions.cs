using CommunityToolkit.Maui.Markup;

namespace Shadcn.Maui.Controls;

public static class ShadcnMauiAppBuilderExtensions
{
    public static MauiAppBuilder UseShadcnMauiControls(this MauiAppBuilder builder)
    {
        builder.UseMauiCommunityToolkitMarkup();
        builder.ConfigureFonts(x =>
        {
            x.AddFont("Lucide.ttf", "Lucide");
        });

        return builder;
    }
}
