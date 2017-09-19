using System.IO;

namespace H.Versioning.VersionProviders
{
    public sealed class FileVersionProviderSettings
    {
        private string versionFilePath;
        private FileInfo versionFile;

        internal FileVersionProviderSettings() { }

        public string VersionFilePath
        {
            get
            {
                return versionFilePath;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    versionFilePath = null;
                    versionFile = null;
                    return;
                }
                versionFilePath = value;
                versionFile = new FileInfo(value);
            }
        }

        public FileInfo VersionFile => versionFile;
    }
}
