using H.Versioning.VersionProviders;
using System.Collections.Generic;

namespace H.Versioning
{
    public class ReleaseVersion
    {
        public static readonly IProvideReleaseVersions ProviderForGit = new GitReleaseVersionsProvider();

        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public KeyValuePair<string, string>[] Notes { get; set; }
        public Version Version { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return Version?.ToString();

            return $"{Name} ({(string.IsNullOrWhiteSpace(Description) ? "~ No Description ~" : Description)})";
        }

        public static implicit operator Version(ReleaseVersion releaseVersion) => releaseVersion.Version;
        public static implicit operator ReleaseVersion(Version version) => new ReleaseVersion { ID = version?.Commit, Name = version?.Number?.ToString(), Version = version };
    }
}
