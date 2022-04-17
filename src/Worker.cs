using Microsoft.Extensions.Logging;

class Worker
{
    private readonly ILogger _logger;
    private readonly IRemoteRepoClient _repoClient;
    private readonly AppSettings _settings;
    private readonly IGitClient _gitClient;

    public Worker(
        ILogger<Worker> logger,
        IRemoteRepoClient repoClient,
        IGitClient gitClient,
        AppSettings appSettings
    )
    {
        _logger = logger;
        _repoClient = repoClient;
        _settings = appSettings;
        _gitClient = gitClient;
    }

    public async Task PerformBackup()
    {
        ValidateSettings();
        
        _logger.LogInformation("List user repositories");
        var repos = await _repoClient.FetchRepositories();
        _logger.LogInformation($"Discovered {repos.Count()} repositories");

        var backupFolder = _settings.BackupFolder;
        _logger.LogInformation($"Look existing backups in {backupFolder}");
        var existingRepos = Directory.EnumerateDirectories(backupFolder).Select(f => f.ToLowerInvariant());
        _logger.LogInformation($"Discovered {existingRepos.Count()} folders");

        foreach (var repo in repos)
        {
            _logger.LogInformation($"Backing up {repo.Name}");
            
            if (existingRepos.Contains(repo.GetPath(backupFolder).ToLowerInvariant()))
            {
                await _gitClient.Fetch(repo);
            }
            else
            {
                await _gitClient.Clone(repo);
                
            }
        }
        _logger.LogInformation("Finished");
    }

    private void ValidateSettings()
    {
        var errors = new List<string>();
        if (string.IsNullOrEmpty(_settings.GitHubToken))
        {
            errors.Add("GitHubToken should not be empty");
        }

        if (string.IsNullOrEmpty(_settings.BackupFolder))
        {
            errors.Add("BackupFolder should not be empty");
        }

        if (!Directory.Exists(_settings.BackupFolder))
        {
            errors.Add($"BackupFolder doesn't exists: {_settings.BackupFolder}");
        }

        if (errors.Count > 0)
        {
            throw new ArgumentException($"ValidateSettings: {string.Join("; ", errors)}");
        }
    }
}