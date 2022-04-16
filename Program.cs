
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<AppSettings>();
        services.AddTransient<GithubBackupWorker>();
        services.AddHostedService<GithubBackupApp>();
    })
    .Build();

await host.RunAsync();