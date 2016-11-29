using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace H.Versioning.Cli
{
    internal class NuspecFile
    {
        private readonly FileInfo nuspecFile;

        public NuspecFile(string path)
        {
            this.nuspecFile = new FileInfo(path);
            if (!this.nuspecFile.Exists)
            {
                throw new FileNotFoundException("The specified *.nuspec file does not exist", path);
            }
        }

        public void UpdateVersion()
        {
            var doc = XDocument.Load(nuspecFile.FullName);
            doc
                .Descendants("package")
                .Descendants("metadata")
                .Descendants("version")
                .Single()
                .SetValue(Version.Self.GetCurrent().ToNuget());
            doc.Save(nuspecFile.FullName);
        }
    }
}