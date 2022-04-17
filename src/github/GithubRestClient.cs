using Octokit;

class GithubRestClient: IRepoClient
{
    private readonly AppSettings _appSettings;

    public GithubRestClient(
        AppSettings appSettings
    )
    {
        _appSettings = appSettings;
    }

    public async Task<IEnumerable<IRepoClient.Repo>> ListUserRepositories()
    {
        var client = new GitHubClient(new ProductHeaderValue("github-backup-app"));
        client.Credentials = new Credentials(_appSettings.GitHubToken);
        var options = new ApiOptions()
        {
           PageSize = 100 
        };
        var repositories = await client.Repository.GetAllForCurrent(options: options);

        return repositories.Select(r => new IRepoClient.Repo()
        {
            Name = r.Name,
            CloneUrl = r.CloneUrl
        });
    }
}