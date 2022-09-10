namespace CoreImprove.Infra.Models;

public record Role
{
	public uint Id { get; private set; }

	public string Name { get; private set; }

	public uint Level { get; private set; }

	public static Role Define(uint Id, string Name, uint Level)
	{
		return new Role
		{
			Id = Id,
			Name = Name,
			Level = Level
		};
	}
}
