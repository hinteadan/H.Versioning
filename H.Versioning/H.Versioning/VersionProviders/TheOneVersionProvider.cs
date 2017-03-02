using System;
using System.Configuration;
using System.IO;

namespace H.Versioning.VersionProviders
{
    internal sealed class TheOneVersionProvider : VersionProviderPipeline
    {
        private static readonly string versionFile = ConfigurationManager.AppSettings["H.Versioning.VersionFile"] ?? "version.txt";

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
    }
}
