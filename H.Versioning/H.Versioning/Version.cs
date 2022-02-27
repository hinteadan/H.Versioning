using H.Versioning.VersionNumberParsers;
using H.Versioning.VersionProviders;
using System;
using System.Globalization;
using System.Linq;

namespace H.Versioning
{
    public sealed class Version
    {
        public static readonly IProvideVersion Self = new TheOneVersionProvider();

        public static readonly Version Unknown = new Version(VersionNumber.Unknown, DateTime.MinValue, "N/A", "N/A");

        private static readonly string[] possibleVersionPartsSeparators = new string[] { "\r", "\n", "\t", "; ", ", ", "| " };

        public readonly VersionNumber Number;
        public readonly DateTime Timestamp;
        public readonly string Branch;
        public readonly string Commit;

        public Version(VersionNumber number, DateTime timestamp, string branch, string commit)
        {
            this.Number = number;
            this.Timestamp = EnsureUtc(timestamp);
            this.Branch = branch;
            this.Commit = commit;
        }

        public override string ToString()
        {
            return ToString(Environment.NewLine);
        }

        public string ToString(string separator)
        {
            return $"{Number}{separator}{Timestamp.ToString(CultureInfo.InvariantCulture)}{separator}{Branch}{separator}{Commit}";
        }

        public static Version Parse(string versionString)
        {
            if (string.IsNullOrWhiteSpace(versionString) || !versionString.Contains("."))
            {
                throw new InvalidOperationException("The given version string does not have the expected format");
            }

            var parts = versionString.Split(possibleVersionPartsSeparators, StringSplitOptions.RemoveEmptyEntries);

            var number = VersionNumber.Parse(parts[0].Trim());
            var timestamp = DateTime.MinValue;
            string branch = parts.Length > 2 ? parts[2].Trim() : null;
            string commit = parts.Length > 3 ? parts[3].Trim() : null;
            if (parts.Length > 1)
            {
                DateTime.TryParse(parts[1].Trim(), CultureInfo.InvariantCulture, DateTimeStyles.None, out timestamp);
            }

            return new Version(number, timestamp, branch, commit);
        }

        public static void UseParser(params ICanParseVersionNumber[] parsers)
        {
            VersionNumber.Use(parsers);
        }

        public static void UseParser(params Func<string, VersionNumber>[] parsers)
        {
            VersionNumber.Use(parsers.Select(p => new DelegateVersionParser(p)).ToArray());
        }

        public static void IgnoreTag(params Predicate<string>[] predicate)
        {
            GitVersionProvider.Ignore(predicate);
        }

        private static DateTime EnsureUtc(DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
                return dateTime;

            if (dateTime.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

            return dateTime.ToUniversalTime();
        }
    }
}