using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H.Versioning.VersionNumberParsers
{
    public class SemanticVersionParser : ICanParseVersionNumber
    {
        public VersionNumber Parse(string versionNumber)
        {
            if (string.IsNullOrWhiteSpace(versionNumber) || !versionNumber.Contains("."))
            {
                throw new InvalidOperationException("The given version is not in semantic format");
            }

            var versionAndSuffixParts = versionNumber.Split(new char[] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries);
            var versionString = versionAndSuffixParts[0].ToLowerInvariant().Replace("v", string.Empty).Trim();
            string suffix = versionAndSuffixParts.Length > 1 ? versionAndSuffixParts[1].Trim() : null;

            var versionParts = versionString.Split(new char[] { '.' }, 4, StringSplitOptions.RemoveEmptyEntries);
            var major = int.Parse(versionParts[0].Trim());
            var minor = int.Parse(versionParts[1].Trim());
            var patch = versionParts.Length > 2 ? (int?)int.Parse(versionParts[2].Trim()) : null;
            var build = versionParts.Length > 3 ? (int?)int.Parse(versionParts[3].Trim()) : null;

            return new VersionNumber(major, minor, patch, build, suffix);
        }
    }
}
