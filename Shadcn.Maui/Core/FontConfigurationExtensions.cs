using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shadcn.Maui.Core;

public static class FontConfigurationExtensions
{
    public static IFontCollection AddLucideIconFonts(this IFontCollection fonts)
    {
        fonts.AddEmbeddedResourceFont(
            typeof(FontConfigurationExtensions).Assembly,
           filename: "lucide.ttf",
           alias: "Lucide");
        
        return fonts;
    }
}
