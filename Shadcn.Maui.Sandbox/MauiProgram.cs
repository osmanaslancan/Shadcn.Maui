using Microsoft.Extensions.Logging;
using Shadcn.Maui.Controls;

namespace Shadcn.Maui.Sandbox
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseShadcnMauiControls()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Geist-Thin.ttf", "GeistThin");
                    fonts.AddFont("Geist-UltraLight.ttf", "GeistUltraLight");
                    fonts.AddFont("Geist-Light.ttf", "GeistLight");
                    fonts.AddFont("Geist-Regular.ttf", "GeistRegular");
                    fonts.AddFont("Geist-Regular.ttf", "Geist");
                    fonts.AddFont("Geist-Medium.ttf", "GeistMedium");
                    fonts.AddFont("Geist-SemiBold.ttf", "GeistSemiBold");
                    fonts.AddFont("Geist-Bold.ttf", "GeistBold");
                    fonts.AddFont("Geist-Black.ttf", "GeistBlack");
                    fonts.AddFont("Geist-UltraBlack.ttf", "GeistUltraBlack");
                });
            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
