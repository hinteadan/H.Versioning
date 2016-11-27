using System;
using System.IO;

namespace H.Versioning.VersionProviders
{
    public sealed class TheOneVersionProvider : VersionProviderPipeline
    {
        public TheOneVersionProvider()
            : base(
                  Pipe(() => File.Exists("version.txt"), new FileVersionProvider("version.txt")),
                  Pipe(() => true, new GitVersionProvider(AppDomain.CurrentDomain.BaseDirectory))
                  )
        { }

        private static Tuple<Func<bool>, IProvideVersion> Pipe(Func<bool> predicate, IProvideVersion provider)
        {
            return Tuple.Create(predicate, provider);
        }
    }
}
