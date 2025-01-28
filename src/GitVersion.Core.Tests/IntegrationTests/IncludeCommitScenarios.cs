using GitVersion.Configuration;
using GitVersion.Core.Tests.Helpers;

namespace GitVersion.Core.Tests.IntegrationTests;

[TestFixture]
public class IncludeCommitScenarios : TestBase
{
    [Test]
    public void ShouldThrowGitVersionExceptionWhenAllCommitsAreIgnored()
    {
        using var fixture = new MonoRepositoryFixture();
        fixture.MakeACommit();

        var configuration = GitFlowConfigurationBuilder.New
            .WithIncludeConfiguration(new IncludeConfiguration() { Paths = ["FILE_DOES_NOT_EXIST"] }).Build();

        Should.Throw<GitVersionException>(() => fixture.GetVersion(configuration))
            .Message.ShouldBe("No commits found on the current branch.");
    }

    [TestCase(null, "0.0.1-1")]
    [TestCase("0.0.1", "0.0.1-1")]
    [TestCase("0.1.0", "0.1.0-1")]
    [TestCase("1.0.0", "1.0.0-1")]
    public void ShouldNotFallbackToBaseVersionWhenSomeCommitsAreIncluded(string? nextVersion, string expectedFullSemVer)
    {
        using var fixture = new MonoRepositoryFixture();
        fixture.MakeAFileCommit("moduleA/file1.txt");
        fixture.MakeAFileCommit("moduleB/file1.txt");

        var configuration = GitFlowConfigurationBuilder.New.WithNextVersion(nextVersion)
            .WithIncludeConfiguration(new IncludeConfiguration() { Paths = ["moduleA/.*"]}).Build();

        fixture.AssertFullSemver(expectedFullSemVer, configuration);
    }

    [TestCase(null, "0.0.1-1")]
    [TestCase("0.0.1", "0.0.1-1")]
    [TestCase("0.1.0", "0.1.0-1")]
    [TestCase("1.0.0", "1.0.0-1")]
    public void ShouldHaveSameVersionWhenOneCommitIsIncluded(string? nextVersion, string expectedFullSemVer)
    {
        using var fixture = new MonoRepositoryFixture();
        fixture.MakeAFileCommit("moduleA/file1.txt");
        fixture.MakeAFileCommit("moduleB/file1.txt");

        var configurationA = GitFlowConfigurationBuilder.New.WithNextVersion(nextVersion)
            .WithIncludeConfiguration(new IncludeConfiguration() { Paths = ["moduleA/.*"]}).Build();
        var configurationB = GitFlowConfigurationBuilder.New.WithNextVersion(nextVersion)
            .WithIncludeConfiguration(new IncludeConfiguration() { Paths = ["moduleB/.*"]}).Build();

        fixture.AssertFullSemver(expectedFullSemVer, configurationA);
        fixture.AssertFullSemver(expectedFullSemVer, configurationB);
    }
}
