using System.Text.RegularExpressions;
using GitVersion.Extensions;
using GitVersion.Git;

namespace GitVersion.Configuration;

internal static class IncludeConfigurationExtensions
{
    public static IEnumerable<ITag> Filter(this IIncludeConfiguration include, ITag[] source, IGitRepository repository)
    {
        include.NotNull();
        source.NotNull();

        return !include.IsEmpty ? source.Where(element => ShouldBeIncluded(element.Commit, include, repository)) : source;
    }

    public static IEnumerable<ICommit> Filter(this IIncludeConfiguration include, ICommit[] source, IGitRepository repository)
    {
        include.NotNull();
        source.NotNull();

        return !include.IsEmpty ? source.Where(element => ShouldBeIncluded(element, include, repository)) : source;
    }

    private static bool ShouldBeIncluded(ICommit commit, IIncludeConfiguration include, IGitRepository repository)
    {
        var regexs = include.Paths.Select(x => new Regex(x, RegexOptions.IgnoreCase | RegexOptions.Compiled))
            .ToArray();
        var changedFiles = repository.GetChangedFiles(commit);
        var hasMatchingPath = regexs.Length == 0 || regexs.Any(regex => changedFiles.Any(regex.IsMatch));

        return hasMatchingPath;
    }
}
