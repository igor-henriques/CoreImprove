using CoreImprove.Infra.DiscordIntegration;
using System.Diagnostics;

namespace CoreImprove.Infra.Models;

public class Settings
{
	public static Role CharInformation;
	public static readonly bool IsSenderFeatureActive = true;
	public static readonly bool IsDiscordFeatureActive = true;
	public static readonly string ClientName = "Toronto";
	public static readonly string DiscordClientID = "999752250303987722";
	public static readonly string Description = $"pwtoronto.com";
	public static readonly Stopwatch stopwatch = Stopwatch.StartNew();

	public static RichPresence DiscordPresence
	{
		get
		{
			string timePlaying = $"Tempo on-line: {GetTimePlaying()}";
			string details = CharInformation?.Name == null ? Description : ("Jogando como " + CharInformation?.Name);
			string state = CharInformation?.Level == null ? timePlaying : $"Level: {CharInformation.Level}\n{timePlaying}";

			RichPresence result = default(RichPresence);
			result.state = state;
			result.details = details;
			result.largeImageKey = "imagem_2022-07-21_164836516";
			result.largeImageText = "http://www.pwtoronto.com";

			return result;
		}
	}

	public static void SetCharInformation(uint id, string charName, uint level)
	{
		CharInformation = Role.Define(id, charName, level);
	}

	private static string GetTimePlaying()
	{
		string hours = stopwatch.Elapsed.Hours.ToString("00");
        string minutes = stopwatch.Elapsed.Minutes.ToString("00");
        string seconds = stopwatch.Elapsed.Seconds.ToString("00"); 

		return $"{hours}:{minutes}:{seconds}";

	}
}
