using System.IO;

namespace H.Versioning.VersionProviders
{
    internal sealed class FileVersionProvider : IProvideVersion
    {
        private readonly FileVersionProviderSettings settings = new FileVersionProviderSettings();

        public FileVersionProviderSettings Settings => settings;

        public FileVersionProvider(string versionFilePath)
        {
            settings.VersionFilePath = versionFilePath;
        }

        public Version GetCurrent()
        {
            if (!settings.VersionFile.Exists)
            {
                return Version.Unknown;
            }

            return Version.Parse(File.ReadAllText(settings.VersionFilePath));
        }
    }
}
