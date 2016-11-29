namespace H.Versioning.Cli
{
    internal static class Extensions
    {
        public static string ToNuget(this Version version)
        {
            return $"{version.Number.Major}.{version.Number.Minor}.{version.Number.Patch ?? 0}{Build(version.Number)}";
        }

        private static string Build(VersionNumber number)
        {
            if (number.Build == null && string.IsNullOrWhiteSpace(number.Suffix))
            {
                return string.Empty;
            }

            return $"-{Clean(number.Suffix)}{number.Build}";
        }

        private static string Clean(string value)
        {
            return value
                .Replace("-", string.Empty)
                .Replace(".", string.Empty);
        }
    }
}
