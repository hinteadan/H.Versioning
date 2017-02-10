using System;
using System.Text;
using H.Versioning.VersionNumberParsers;

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
            return new SemanticVersionParser().Parse(semanticVersion);
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