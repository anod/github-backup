
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<AppSettings>();
        services.AddScoped<IRemoteRepoClient, OctokitRestClient>();
        services.AddScoped<IGitClient, LibGit2Client>();
        services.AddScoped<Worker>();
        services.AddHostedService<App>();
    })
    .Build();

await host.RunAsync();