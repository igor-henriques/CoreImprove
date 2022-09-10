using System;

namespace CoreImprove.Infra.Models;

public class ClientEventArgs : EventArgs
{
	public string Name;

	public string Class;

	public ClientEventArgs(string name, int _class, int level)
	{
		Name = name;
		string text = "";
		switch (_class)
		{
		case 0:
			text = "Blademaster";
			break;
		case 1:
			text = "Wizard";
			break;
		case 2:
			text = "Psychic";
			break;
		case 3:
			text = "Venomancer";
			break;
		case 4:
			text = "Barbarian";
			break;
		case 5:
			text = "Assassin";
			break;
		case 6:
			text = "Archer";
			break;
		case 7:
			text = "Cleric";
			break;
		case 8:
			text = "Seeker";
			break;
		case 9:
			text = "Mystic";
			break;
		case 10:
			text = "Stormbringer";
			break;
		case 11:
			text = "Duskblade";
			break;
		}
		Class = text + " Lv. " + level;
	}
}
