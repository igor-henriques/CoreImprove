namespace CoreImprove.Infra.DiscordIntegration;

public struct EventHandlers
{
	public DiscordRpc.ReadyCallback readyCallback;

	public DiscordRpc.DisconnectedCallback disconnectedCallback;

	public DiscordRpc.ErrorCallback errorCallback;
}
