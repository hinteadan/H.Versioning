using System.IO;

namespace H.Versioning.VersionProviders
{
    internal sealed class FileVersionProvider : IProvideVersion
    {
        private readonly FileInfo versionFile;

        public FileVersionProvider(string versionFilePath)
        {
            this.versionFile = new FileInfo(versionFilePath);
        }

        public Version GetCurrent()
        {
            if (!this.versionFile.Exists)
            {
                return Version.Unknown;
            }

            return Version.Parse(File.ReadAllText(versionFile.FullName));
        }
    }
}
