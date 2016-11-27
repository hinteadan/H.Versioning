using System;
using System.Text;

namespace H.Versioning
{
    public sealed class VersionNumber
    {
        public static readonly VersionNumber Unknown = new VersionNumber(0, 0, 0, 0, "unknown");

        public readonly int Major;
        public readonly int Minor;
        public readonly int? Patch;
        public readonly int? Build;
        public readonly string Suffix;

        public VersionNumber(int major, int minor, int? patch, int? build, string suffix)
        {
            this.Major = major;
            this.Minor = minor;
            this.Patch = patch;
            this.Build = build;
            this.Suffix = suffix;
        }
        public VersionNumber(int major, int minor) : this(major, minor, null, null, null) { }
        public VersionNumber(int major, int minor, int patch) : this(major, minor, patch, null, null) { }
        public VersionNumber(int major, int minor, int patch, int build) : this(major, minor, patch, build, null) { }

        public static VersionNumber Parse(string semanticVersion)
        {
            if (string.IsNullOrWhiteSpace(semanticVersion) || !semanticVersion.Contains("."))
            {
                throw new InvalidOperationException("The given version is not in semantic format");
            }

            var versionAndSuffixParts = semanticVersion.Split(new char[] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);
            var versionString = versionAndSuffixParts[0].ToLowerInvariant().Replace("v", string.Empty).Trim();
            string suffix = versionAndSuffixParts.Length > 1 ? versionAndSuffixParts[1].Trim() : null;

            var versionParts = versionString.Split(new char[] { '.' }, 4, StringSplitOptions.RemoveEmptyEntries);
            var major = int.Parse(versionParts[0].Trim());
            var minor = int.Parse(versionParts[1].Trim());
            var patch = versionParts.Length > 2 ? (int?)int.Parse(versionParts[2].Trim()) : null;
            var build = versionParts.Length > 3 ? (int?)int.Parse(versionParts[3].Trim()) : null;

            return new VersionNumber(major, minor, patch, build, suffix);
        }

        public override string ToString()
        {
            var versionString = new StringBuilder($"{this.Major}.{this.Minor}");

            if (this.Patch != null)
            {
                versionString.Append($".{this.Patch}");

                if (this.Build != null)
                {
                    versionString.Append($".{this.Build}");
                }
            }

            if (!string.IsNullOrWhiteSpace(this.Suffix))
            {
                versionString.Append($"-{this.Suffix}");
            }

            return versionString.ToString();
        }
    }
}