using System.Text.Json;

namespace MyProject;

class Program
{
	static void Main()
	{
		FootballTeamSnakeCased team = new FootballTeam
		{
			ID = 1,
			TeamName = "Manchester United",
			HeadCoach = "Ole Gunnar Solskjær",
			HomeStadium = "Old Trafford"
		}.ToSnakeCased();

		PersonSnakeCased person = new Person
		{
			FirstName = "John",
			DateOfBirth = new DateTime(1980, 1, 1),
			Age = 42,
			MiddleName = "James"
		}.ToSnakeCased();

		Console.WriteLine(JsonSerializer.Serialize(team));
		Console.WriteLine(JsonSerializer.Serialize(person));
	}
}
