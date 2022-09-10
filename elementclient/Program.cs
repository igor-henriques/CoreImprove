using CoreImprove.App.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder().ConfigureServices(services => 
{
    services.AddHostedService<SenderWorker>();
    services.AddHostedService<DiscordWorker>();
    services.AddHostedService<KeepAliveWorker>();
}).Build()
  .RunAsync();