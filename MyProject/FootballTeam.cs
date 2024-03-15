using MySourceGenerator;
using MySourceGenerator.Attributes;

namespace MyProject;

[PascalToSnake]
public class FootballTeam
{
	public FootballTeam()
	{
		TeamName = "";
		HeadCoach = "";
		HomeStadium = "";
	}

	public int ID { get; set; }
	public string TeamName { get; set; }
	public string HeadCoach { get; set; }
	public string HomeStadium { get; set; }

	[PascalToSnakeIgnore]
	public int UpdatedByUserID { get; set; }
}
