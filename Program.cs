
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) => {
        services.AddSingleton<AppSettings>();
        services.AddTransient<IRepoClient, GithubRestClient>();
        services.AddTransient<Worker>();
        services.AddHostedService<App>();
    })
    .Build();

await host.RunAsync();