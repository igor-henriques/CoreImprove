await Host.CreateDefaultBuilder().ConfigureServices(services => 
{
    services.AddHostedService<SenderWorker>();
    services.AddHostedService<DiscordWorker>();
    services.AddHostedService<KeepAliveWorker>();
}).Build().RunAsync();