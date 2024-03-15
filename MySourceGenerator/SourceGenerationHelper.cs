using System.Text;

namespace MySourceGenerator;

internal static class SourceGenerationHelper
{
	public static string GenerateNewCode(TypeToGenerate typeToGenerate)
	{
		string ns = typeToGenerate.GetNamespace();
		if (!string.IsNullOrEmpty(ns))
		{
			ns = $"namespace {ns};";
		}

		StringBuilder propertyLines = new();
		StringBuilder assignmentLines = new();
		foreach (PropertyData prop in typeToGenerate.Properties)
		{
			if (prop.IsIgnored) { continue; }
			string snakeCasePropName = ConvertToSnakeCase(prop.Name);
			propertyLines.AppendLine($"\tpublic {prop.Type} {snakeCasePropName} {{ get; set; }}");
			assignmentLines.AppendLine($"\t\t\t{snakeCasePropName} = value.{prop.Name},");
		}

		return $$"""
		{{ns}}

		public {{typeToGenerate.TypeType}} {{typeToGenerate.GetTypeName()}}SnakeCased
		{
		{{propertyLines.ToString().TrimEnd()}}
		}

		public static partial class MySourceGeneratorExtensions
		{
			public static {{typeToGenerate.GetTypeName()}}SnakeCased ToSnakeCased(this {{typeToGenerate.GetTypeName()}} value)
			{
				return new {{typeToGenerate.GetTypeName()}}SnakeCased
				{
		{{assignmentLines.ToString().TrimEnd('\r', '\n', ',')}}
				};
			}
		}

		""";
	}

	private static string ConvertToSnakeCase(string input)
	{
		if (input.Length < 2) { return input.ToLower(); }

		StringBuilder sb = new();

		sb.Append(char.ToLower(input[0]));

		for (int i = 1; i < input.Length; ++i)
		{
			if (char.IsUpper(input[i]) && !char.IsUpper(input[i - 1]))
			{
				sb.Append('_');
			}

			sb.Append(char.ToLower(input[i]));
		}

		return sb.ToString();
	}
}
