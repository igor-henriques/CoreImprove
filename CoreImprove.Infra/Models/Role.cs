namespace CoreImprove.Infra.Models;

public record Role
{
	public static uint Id { get; private set; }
	public static string Name { get; private set; }
	public static uint Level { get; private set; }
	public static string Occupation { get; private set; }

	public static void SetProperties(uint id = default, string name = default, uint level = default, string occupation = default)
	{
		if (id != default) Id = id;
		if (name != default) Name = name;
		if (level != default) Level = level;
		if (occupation != default) Occupation = occupation;
	}
}
