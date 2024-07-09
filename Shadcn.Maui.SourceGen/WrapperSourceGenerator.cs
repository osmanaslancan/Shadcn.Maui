using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace Shadcn.Maui.SourceGen;

[Generator]
class WrapperSourceGenerator : IIncrementalGenerator
{
    private record BindableProperty(IFieldSymbol Field, IMethodSymbol? TargetType);
    private record Model(string Name, string Namespace, INamedTypeSymbol targetType, List<BindableProperty> fieldsToGenerate, List<BindableProperty> existingFields);

    private static string RemovePropertyPostfix(string name)
    {
        return name.Substring(0, name.Length - "Property".Length);
    }

    public void Initialize(IncrementalGeneratorInitializationContext initContext)
    {
        var combinedProviders = initContext.SyntaxProvider.ForAttributeWithMetadataName(
           "Shadcn.Maui.Common.WrapsControlAttribute",
           predicate: static (node, _) => node is ClassDeclarationSyntax m && m.AttributeLists.Count > 0,
               transform: (context, _) =>
               {
                   var syntaxNode = (INamedTypeSymbol)context.TargetSymbol;
                   

                   var attributeSyntax = syntaxNode.GetAttributes().First(x => x.AttributeClass!.MetadataName == "WrapsControlAttribute");
                   var targetType = attributeSyntax.ConstructorArguments.First().Value as INamedTypeSymbol;
                   
                   IEnumerable<ISymbol> getRecursiveMembers(INamedTypeSymbol symbol)
                   {
                       var current = symbol;
                       while(current is not null)
                       {
                           foreach (var member in current.GetMembers())
                           {
                               yield return member;
                           }
                           current = current.BaseType;
                       }
                   }
                   var recursiveMembers = getRecursiveMembers(targetType!).ToList();
                   var fieldsToGenerate = targetType!.GetMembers()
                   .Where(x => x is IFieldSymbol fs && x.IsStatic && x.Name.EndsWith("Property"))
                   .OfType<IFieldSymbol>()
                   .Select(x => new BindableProperty(x, recursiveMembers.FirstOrDefault(y => y.Name == "get_" + RemovePropertyPostfix(x.Name)) as IMethodSymbol)).ToList();
                   var className = syntaxNode.Name;
                   var namespaceName = syntaxNode.ContainingNamespace.ToDisplayString();
                   var existingFields = syntaxNode.GetMembers().Where(x => x.IsStatic && x.Name.EndsWith("Property")).OfType<IFieldSymbol>()
                   .Select(x => new BindableProperty(x, targetType.GetMembers().First(y => y.Name == "get_" + RemovePropertyPostfix(x.Name)) as IMethodSymbol)).ToList(); 

                   return new Model(className, namespaceName, targetType, fieldsToGenerate, existingFields);
               })
               .Where(static m => m is not null)
               .Combine(initContext.CompilationProvider);

        initContext.RegisterSourceOutput(combinedProviders, (spc, providers) =>
        {
            var model = providers.Left;
            var compilation = providers.Right;


            var source = $$"""
            using CommunityToolkit.Maui.Markup;

            namespace {{model.Namespace}};
            public partial class {{model.Name}}
            {
            {{GetBindMethod(model)}}
            {{GetBindableProperties(model)}}
            }
            """;
            
            spc.AddSource($"{model.Name}Wrap{model.targetType.Name}.g.cs", SourceText.From(source, Encoding.UTF8));

        });
    }

    private string GetBindableProperties(Model model)
    {
        var sb = new StringBuilder();

        foreach (var bindableProperty in model.fieldsToGenerate)
        {
            var field = bindableProperty.Field;
            var existingField = model.existingFields.Select(x => x.Field).FirstOrDefault(x => x.Name == field.Name);

            if (existingField is not null && existingField.ContainingType.Name == model.Name)
            {
                continue;
            }

            var type = bindableProperty.TargetType!.ReturnType.ToString();

            sb.AppendLine($@"public {(existingField is not null ? "new" : "")}static readonly BindableProperty {field.Name} = BindableProperty.Create(""{field.Name.Substring(0, field.Name.Length - "Property".Length)}"", typeof({type}), typeof({model.Name}), default({type}), defaultBindingMode: BindingMode.TwoWay);");
            sb.AppendLine($$"""
            public {{type}} {{field.Name.Substring(0, field.Name.Length - "Property".Length)}} 
            {
                get => ({{type}})GetValue({{field.Name}});
                set => SetValue({{field.Name}}, value); 
            }
            """);
        }

        return sb.ToString();
    }

    private string GetBindMethod(Model model)
    {
        var sb = new StringBuilder();

        sb.AppendLine($"private void BindWrapped{model.targetType.Name}({model.targetType.MetadataName} wrapped) {{");
        foreach (var bindableProperty in model.fieldsToGenerate)
        {
            var field = bindableProperty.Field;
            var existingField = model.existingFields.Select(x => x.Field).FirstOrDefault(x => x.Name == field.Name);

            if (existingField is not null && existingField.ContainingType.Name == model.Name)
            {
                continue;
            }

            sb.AppendLine($@"wrapped.Bind({model.targetType}.{field.Name}, ""{field.Name.Substring(0, field.Name.Length - "Property".Length)}"", source: this, mode: BindingMode.TwoWay);");
        }
        sb.AppendLine("}");
        return sb.ToString();
    }
}
