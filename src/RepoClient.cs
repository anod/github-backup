interface IRepoClient
{
    public sealed class Repo
    {
        public string Name = "";
        public string CloneUrl = "";
    }

     Task<IEnumerable<Repo>> ListUserRepositories();
}