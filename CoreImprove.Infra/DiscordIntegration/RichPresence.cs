using System;

namespace CoreImprove.Infra.DiscordIntegration;

[Serializable]
public struct RichPresence
{
	public string state;

	public string details;

	public long startTimestamp;

	public long endTimestamp;

	public string largeImageKey;

	public string largeImageText;

	public string smallImageKey;

	public string smallImageText;

	public string partyId;

	public int partySize;

	public int partyMax;

	public string matchSecret;

	public string joinSecret;

	public string spectateSecret;

	public bool instance;
}
