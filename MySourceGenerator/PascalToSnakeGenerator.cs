using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace MySourceGenerator;

[Generator]
public class PascalToSnakeGenerator
	: IIncrementalGenerator
{
	void IIncrementalGenerator.Initialize(IncrementalGeneratorInitializationContext context)
	{
		IncrementalValuesProvider<TypeToGenerate?> typesToGenerate = context.SyntaxProvider
			.ForAttributeWithMetadataName(
				"MySourceGenerator.Attributes.PascalToSnakeAttribute",
				predicate: static (s, _) => true,
				transform: static (ctx, _) => GetTypeToGenerate(ctx.SemanticModel, ctx.TargetNode)
			).Where(static m => m is not null);

		context.RegisterSourceOutput(typesToGenerate,
			static (spc, source) => Execute(source, spc));
	}

	private static void Execute(TypeToGenerate? typeToGenerate, SourceProductionContext context)
	{
		if (typeToGenerate is not null)
		{
			string result = SourceGenerationHelper.GenerateNewCode(typeToGenerate.Value);
			context.AddSource($"PascalToSnake.{typeToGenerate.Value.Name}.g.cs", SourceText.From(result, Encoding.UTF8));
		}
	}

	private static TypeToGenerate? GetTypeToGenerate(SemanticModel semanticModel, SyntaxNode declarationSyntax)
	{
		if (semanticModel.GetDeclaredSymbol(declarationSyntax) is not INamedTypeSymbol symbol)
		{
			return null;
		}

		string typeName = symbol.ToString();

		ImmutableList<PropertyData> propertyTypes = symbol.GetMembers()
			.OfType<IPropertySymbol>()
			.Where(static m => m.SetMethod is not null)
			.Select(p => new PropertyData(
				Name: p.Name,
				Type: p.Type.ToDisplayString(),
				IsIgnored: p.GetAttributes().Any(static a => a.AttributeClass?.ToString() == "MySourceGenerator.Attributes.PascalToSnakeIgnoreAttribute")
			)
		).ToImmutableList();

		string typeType = symbol.TypeKind switch
		{
			TypeKind.Class => "class",
			TypeKind.Struct => "struct",
			_ => throw new InvalidDataException("Only classes and structs are supported.")
		};

		return new TypeToGenerate(typeType, typeName, propertyTypes); 
	}
}
