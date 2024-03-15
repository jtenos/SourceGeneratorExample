using MySourceGenerator.Attributes;

namespace MyProject;

[PascalToSnake]
public struct Person
{
	public required string FirstName { get; set; }
	public DateTime? DateOfBirth { get; set; }
	public int? Age { get; set; }

	[PascalToSnakeIgnore]
	public string? MiddleName { get; set; }
}
