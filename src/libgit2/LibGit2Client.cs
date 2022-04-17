using LibGit2Sharp;
using Microsoft.Extensions.Logging;

class LibGit2Client : IGitClient
{
    private readonly ILogger _logger;
    private readonly AppSettings _settings;
    private readonly string _backupFolder;
    
    public LibGit2Client(
        ILogger<LibGit2Client> logger,
        AppSettings appSettings
    )
    {
        _logger = logger;
        _settings = appSettings;
        _backupFolder = appSettings.BackupFolder;
    }

    public Task Clone(IRemoteRepoClient.RemoteRepo repo)
    {
        var repoPath = repo.GetPath(_backupFolder);
        _logger.LogInformation($"Perform clone into {repoPath}");
        return Task.Run(() =>
        {
            var cloneOptions = new CloneOptions()
            {
                Checkout = false,
                RecurseSubmodules = true,
                OnProgress = (serverProgressOutput) =>
                {
                    _logger.LogInformation($"OnProgress {repo.Name} {serverProgressOutput}");
                    return true;
                },
                FetchOptions = new FetchOptions()
                {
                    TagFetchMode = TagFetchMode.None
                },
                CredentialsProvider = (url, usernameFromUrl, types) => GetCredentials(repo)
            };
            var path = Repository.Clone(repo.CloneUrl, repoPath, cloneOptions);
            _logger.LogInformation($"Finished clone {path}");
        });
    }

    public Task Fetch(IRemoteRepoClient.RemoteRepo repo)
    {
        var repoPath = repo.GetPath(_backupFolder);
        _logger.LogInformation($"Perform fetch in {repoPath}");
        return Task.Run(() =>
        {
            var repository = new Repository(repoPath);
            var fetchOptions = new FetchOptions()
            {
                OnProgress = (serverProgressOutput) =>
                {
                    _logger.LogInformation($"OnProgress {repo.Name} {serverProgressOutput}");
                    return true;
                },
                TagFetchMode = TagFetchMode.None,
                Prune = true,
                CredentialsProvider = (url, usernameFromUrl, types) => GetCredentials(repo)

            };
            Commands.Fetch(repository, "origin", new List<string>() { "+refs/heads/*:refs/remotes/origin/*" }, fetchOptions, "Git backup fetch");
            _logger.LogInformation($"Finished fetching {repoPath}");
        });
    }

    private Credentials GetCredentials(IRemoteRepoClient.RemoteRepo repo)
    {
         return new UsernamePasswordCredentials()
        {
            Username = repo.UserName,
            Password = _settings.GitHubToken
        };
    }
}