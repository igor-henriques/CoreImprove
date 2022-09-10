using System.Threading;
using System.Threading.Tasks;
using CoreImprove.Infra.DiscordIntegration;
using CoreImprove.Infra.Models;
using Microsoft.Extensions.Hosting;

namespace CoreImprove.App.Workers;

internal class DiscordWorker : BackgroundService
{
	private EventHandlers handlers = default(EventHandlers);

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		if (Settings.IsDiscordFeatureActive)
		{
			DiscordRpc.Initialize(Settings.DiscordClientID, ref handlers, autoRegister: true, null);

			while (!stoppingToken.IsCancellationRequested)
			{
                RichPresence presence = Settings.DiscordPresence;
				DiscordRpc.UpdatePresence(ref presence);
				await Task.Delay(1000);
			}
		}
	}
}
