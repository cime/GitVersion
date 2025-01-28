namespace GitVersion.Testing;

public class MonoRepositoryFixture(string branchName = "main") : RepositoryFixtureBase(path => CreateNewRepository(path, branchName))
{
    public string MakeAFileCommit(string relativeFileName)
    {
        var to = Repository.Head.FriendlyName;
        this.SequenceDiagram.MakeACommit(to);
        var commit = Repository.MakeAFileCommit(relativeFileName);
        return commit.Sha;
    }
}
