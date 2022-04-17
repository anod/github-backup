using Microsoft.Extensions.Logging;

class Worker
{
    private readonly ILogger _logger;
    private readonly IRepoClient _repoClient;
    private readonly AppSettings _settings;

    public Worker(
        ILogger<Worker> logger,
        IRepoClient repoClient,
        AppSettings appSettings
    )
    {
        _logger = logger;
        _repoClient = repoClient;
        _settings = appSettings;
    }

    public async Task PerformBackup()
    {
        _logger.LogInformation("List user repositories");
        var repos = await _repoClient.ListUserRepositories();
        _logger.LogInformation($"Discovered {repos.Count()} repositories");
        _logger.LogInformation("Ensure backup folders");

        _logger.LogInformation("Clone or pull");
    }
}