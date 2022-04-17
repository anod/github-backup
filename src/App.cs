using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class App : IHostedService
{
    private readonly ILogger _logger;
    private readonly Worker _worker;

    public App(
        ILogger<App> logger,
        IHostApplicationLifetime appLifetime,
        Worker worker
    )
    {
        _logger = logger;
        _worker = worker;
        
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
                await _worker.PerformBackup();
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