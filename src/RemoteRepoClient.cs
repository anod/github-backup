interface IRemoteRepoClient
{
    public sealed class RemoteRepo
    {
        public string Name = "";
        public string CloneUrl = "";
        public string UserName = "";

        public string GetPath(string basePath)
        {
            return Path.Combine(basePath, Name);
        }
    }

    Task<IEnumerable<RemoteRepo>> FetchRepositories();
}