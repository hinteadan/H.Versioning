using System;
using System.IO;
using System.Reflection;

namespace H.Versioning.VersionProviders
{
    internal sealed class TheOneVersionProvider : VersionProviderPipeline
    {
        private static readonly string versionFile = Path.Combine(GetBaseFolderPath(), "version.txt");

        public TheOneVersionProvider()
            : base(
                  Pipe(() => File.Exists(versionFile), new FileVersionProvider(versionFile)),
                  Pipe(() => true, new GitVersionProvider(AppDomain.CurrentDomain.BaseDirectory))
                  )
        { }

        private static Tuple<Func<bool>, IProvideVersion> Pipe(Func<bool> predicate, IProvideVersion provider)
        {
            return Tuple.Create(predicate, provider);
        }

        private static string GetBaseFolderPath()
        {
            string codeBase = Assembly.GetExecutingAssembly()?.Location ?? string.Empty;
            UriBuilder uri = new UriBuilder(codeBase);
            string dllPath = Uri.UnescapeDataString(uri.Path);
            string dllFolderPath = Path.GetDirectoryName(dllPath) ?? string.Empty;
            return dllFolderPath;
        }
    }
}
