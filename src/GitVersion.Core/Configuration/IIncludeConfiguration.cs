namespace GitVersion.Configuration;

public interface IIncludeConfiguration
{
    IReadOnlySet<string> Paths { get; }

    public bool IsEmpty => Paths.Count == 0;
}
