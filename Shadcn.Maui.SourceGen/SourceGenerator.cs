using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Shadcn.Maui.SourceGen;

[Generator]
#pragma warning disable RS1036 // Specify analyzer banned API enforcement setting
public class ColorOpacityGenerator : IIncrementalGenerator
#pragma warning restore RS1036 // Specify analyzer banned API enforcement setting
{
    private sealed record Model(string Name, string Namespace)
    {
        public XDocument? Xaml { get; set; }
    };
    public void Execute(GeneratorExecutionContext _)
    {
    }
    
    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        var syntaxProvider = initContext.SyntaxProvider.ForAttributeWithMetadataName(
            "Shadcn.Maui.Common.ColorDictionaryAttribute",
            predicate: static (node, _) => node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0,
                transform: (context, _) =>
                {
                    var node = (ClassDeclarationSyntax)context.TargetNode;

                    string className = node.Identifier.ValueText;
                    var namespaceName = node.FirstAncestorOrSelf<BaseNamespaceDeclarationSyntax>()?.Name.ToString();
                    if (namespaceName is null)
                    {
                        return null;
                    }
                    return new Model(className, namespaceName);
                })
                .Where(static m => m is not null)
                .Combine(initContext.AdditionalTextsProvider.Collect())
                .Select((pair, token) =>
                {
                    var xaml = pair.Right.First(x => x.Path.EndsWith(pair.Left!.Name + ".xaml"));
                    var xdoc = XDocument.Load(xaml.Path);
                    pair.Left!.Xaml = xdoc;
                    return pair.Left;
                });

        initContext.RegisterSourceOutput(syntaxProvider,
           static (spc, source) =>
           {
               if (source.Xaml is null)
                   return;
                
               spc.AddSource($"{source.Name}Opacities.g.cs", SourceText.From($$"""
                namespace {{source.Namespace}};
                
                public partial class {{source.Name}}
                {
                    private void AddOpacities()
                    {
                {{GetColorVariants(source.Xaml)}}
                    }
                }
                """, Encoding.UTF8));

           });
    }

    private static string GetColorVariants(XDocument xdoc)
    {
        List<(string key, string color)> colors = xdoc.Descendants()
            .Where(x => x.Name.LocalName == "Color")
            .Select(x => (x.Attributes().FirstOrDefault(x => x.Name.LocalName == "Key").Value, x.Value))
            .Where(x => x.Item1 != null).ToList();

        var sb = new StringBuilder();

        foreach (var (key, color) in colors)
        {
            // For now only support #RRGGBB format
            if (!color.StartsWith("#") || color.Length != 7 || char.IsDigit(key.Last()))
                continue;

            foreach (var item in GenerateNumbers())
            {
                var opacity = ((int)(2.55 * item)).ToString("X");
                var opacityKey = $"{key}{item}";
                if (colors.Any(x => x.key == opacityKey))
                    continue;
                sb.AppendLine($"\t\tAdd(\"{opacityKey}\", Microsoft.Maui.Graphics.Color.FromArgb(\"#{opacity}{color.Substring(1)}\"));");
            }
        }

        return sb.ToString();
    }


    private static IEnumerable<int> GenerateNumbers()
    {
        for (int i = 5; i < 100; i += 5)
        {
            yield return i;
        }
    }


}


