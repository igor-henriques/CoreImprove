using CoreImprove.Infra.DiscordIntegration;
using System.Diagnostics;

namespace CoreImprove.Infra.Models;

public class Settings
{
	public static readonly bool IsSenderFeatureActive = true;
	public static readonly bool IsDiscordFeatureActive = true;
	public static readonly string ClientName = "Toronto";
	public static readonly string DiscordClientID = "999752250303987722";

	private static readonly string Description = $"pwtoronto.com";
	private static readonly Stopwatch stopwatch = Stopwatch.StartNew();
	private static readonly string largeImageKey = "imagem_2022-07-21_164836516";
	private static readonly string website = "http://www.pwtoronto.com";

	public static RichPresence DiscordPresence
	{
		get
		{
			bool hasClass = !string.IsNullOrEmpty(Role.Occupation);
			string timePlaying = $"Tempo on-line: {GetTimePlaying()}";

			string details = Role.Name == null ? Description : ($"Jogando como {Role.Name}");
			string state = Role.Level == default(uint) ? timePlaying : $"Level: {Role.Level} | {GetTimePlaying()}";

			if (hasClass)
				state += $" | Classe: {Role.Occupation}";

            RichPresence result = default(RichPresence);
			result.state = state;
			result.details = details;
			result.largeImageKey = largeImageKey;
			result.largeImageText = website;

			return result;
		}
	}

	private static string GetTimePlaying()
	{
		string hours = stopwatch.Elapsed.Hours.ToString("00");
        string minutes = stopwatch.Elapsed.Minutes.ToString("00");
        string seconds = stopwatch.Elapsed.Seconds.ToString("00"); 

		return $"{hours}:{minutes}:{seconds}";

	}
}
