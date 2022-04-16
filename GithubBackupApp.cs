using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

class GithubBackupApp : IHostedService
{
    private readonly ILogger _logger;
    private readonly GithubBackupWorker _worker;

    public GithubBackupApp(
        ILogger<GithubBackupApp> logger,
        IHostApplicationLifetime appLifetime,
        GithubBackupWorker worker
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
            await _worker.PerformBackup();
            Environment.Exit(0);
        });
    }
}