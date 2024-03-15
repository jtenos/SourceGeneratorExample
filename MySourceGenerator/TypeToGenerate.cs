using System.Collections.Immutable;

namespace MySourceGenerator;

internal record struct TypeToGenerate(string TypeType, string Name, ImmutableList<PropertyData> Properties)
{
	public string GetNamespace()
	{
		string[] fields = Name.Split('.');
		if (fields.Length > 1)
		{
			return string.Join(".", Enumerable.Range(0, fields.Length - 1).Select(i => fields[i]));
		}
		return string.Empty;
	}

	public string GetTypeName()
	{
		string[] fields = Name.Split('.');
		return fields[fields.GetUpperBound(0)];
	}
}

internal record struct PropertyData(string Name, string Type, bool IsIgnored);
