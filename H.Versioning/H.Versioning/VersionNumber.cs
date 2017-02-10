using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using H.Versioning.VersionNumberParsers;

namespace H.Versioning
{
    public sealed class VersionNumber
    {
        private static readonly ConcurrentStack<ICanParseVersionNumber> parsers = new ConcurrentStack<ICanParseVersionNumber>(new ICanParseVersionNumber[] {
            new SemanticVersionParser()
        });

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

        public static VersionNumber Parse(string version)
        {
            List<Exception> exceptions = new List<Exception>();

            foreach (var parser in parsers)
            {
                try
                {
                    return parser.Parse(version);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            throw new AggregateException($"Unable to parse version string \"{version}\" with any of the registered parsers. See inner exceptions for details.", exceptions);
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

        public static void Use(params ICanParseVersionNumber[] parsers)
        {
            VersionNumber.parsers.PushRange(parsers);
        }
    }
}