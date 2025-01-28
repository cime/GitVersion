using GitVersion.Configuration.Attributes;

namespace GitVersion.Configuration;

internal record IncludeConfiguration : IIncludeConfiguration
{
    [JsonIgnore]
    IReadOnlySet<string> IIncludeConfiguration.Paths => Paths;

    [JsonPropertyName("paths")]
    [JsonPropertyDescription("A sequence of paths to be included from the version calculations.")]
    public HashSet<string> Paths { get; init; } = [];
}
