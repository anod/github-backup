interface IGitClient
{
    Task Fetch(IRemoteRepoClient.RemoteRepo repo);
    Task Clone(IRemoteRepoClient.RemoteRepo repo);
}