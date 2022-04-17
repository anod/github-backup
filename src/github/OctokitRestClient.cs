using Octokit;

class OctokitRestClient: IRemoteRepoClient
{
    private readonly AppSettings _appSettings;

    public OctokitRestClient(
        AppSettings appSettings
    )
    {
        _appSettings = appSettings;
    }

    public async Task<IEnumerable<IRemoteRepoClient.RemoteRepo>> FetchRepositories()
    {
        var client = new GitHubClient(new ProductHeaderValue("github-backup-app"));
        client.Credentials = new Credentials(_appSettings.GitHubToken);
        var options = new ApiOptions()
        {
           PageSize = 100 
        };
        var repositories = await client.Repository.GetAllForCurrent(options: options);

        return repositories.Select(r => new IRemoteRepoClient.RemoteRepo()
        {
            Name = r.Name,
            CloneUrl = r.CloneUrl,
            UserName = r.Owner.Login
        });
    }
}