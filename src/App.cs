using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class App : IHostedService
{
    private readonly ILogger _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public App(
        ILogger<App> logger,
        IHostApplicationLifetime appLifetime,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        
        appLifetime.ApplicationStarted.Register(OnStarted);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStarted()
    {
         _logger.LogInformation("App Started");
        Task.Run(async () => 
        {
            try
            {
                using(IServiceScope scope = _serviceScopeFactory.CreateScope())
                {
                    var worker = scope.ServiceProvider.GetRequiredService<Worker>();
                    await worker.PerformBackup();
                }
            } 
            catch (Exception e)
            {
                 _logger.LogError($"Unhandled exception - {e.GetType()}: {e.Message} {e.StackTrace}", e);
            } 
            finally
            {
                Environment.Exit(0);
            }
        });
    }
}