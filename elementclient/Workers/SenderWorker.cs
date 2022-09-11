namespace CoreImprove.App.Workers;

internal class SenderWorker : BackgroundService
{
    private Manager manager;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!Settings.IsSenderFeatureActive)
            return;
        
        Process clientInstance = await StartClient();

        if (IsEnhanceEnabled())
            return;

        if (await LoadEnhance(clientInstance))
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Role.SetProperties(manager.elementclient.UID, manager.elementclient.Name, manager.elementclient.Level, manager.elementclient.Occupation);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }

    private async Task<Process> StartClient()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = Path.Combine(Directory.GetCurrentDirectory(), Settings.ClientName),
            Arguments = "start " + Settings.ClientName + " startbypatcher user: pwd: role: game:cpw console:1 nocheck"
        });

        await Task.Delay(5000);

        return TaskUtils.GetFirstClientProcess();
    }

    private async Task<bool> LoadEnhance(Process clientInstance)
    {
        this.manager = new Manager(clientInstance.Id);

        await TaskUtils.WaitUntil(() => !string.IsNullOrEmpty(this.manager.elementclient.Name), 100);

        if (this.manager.Connected)
        {
            this.manager.Connect();
        }

        return true;
    }

    private bool IsEnhanceEnabled()
    {
        return (from p in Process.GetProcessesByName("elementclient")
                where p.Id != Process.GetCurrentProcess().Id
                select p).Any();
    }
}
