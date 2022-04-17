
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<AppSettings>();
        services.AddScoped<IRepoClient, GithubRestClient>();
        services.AddScoped<Worker>();
        services.AddHostedService<App>();
    })
    .Build();

await host.RunAsync();