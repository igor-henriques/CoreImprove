namespace CoreImprove.App.Workers;

internal class KeepAliveWorker : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			if (TaskUtils.GetFirstClientProcess() == null)
			{
				Process.GetCurrentProcess()?.Kill();
			}

			await Task.Delay(1000, stoppingToken);
		}
	}
}
