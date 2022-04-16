using Microsoft.Extensions.Logging;

class GithubBackupWorker
{
    private readonly ILogger _logger;
    private readonly AppSettings _settings;

    public GithubBackupWorker(
        ILogger<GithubBackupWorker> logger,
        AppSettings appSettings
    )
    {
        _logger = logger;
        _settings = appSettings;
    }

    public Task PerformBackup()
    {
        _logger.LogInformation("List user repositories");

        _logger.LogInformation("Ensure backup folders");

        _logger.LogInformation("Clone or pull");

        return Task.CompletedTask;
    }
}